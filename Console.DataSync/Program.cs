//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="Lifeprojects.de">
//     Class: Program
//     Copyright © Lifeprojects.de 2026
// </copyright>
// <Template>
// 	Version 3.0.2026.2, 15.04.2026
// </Template>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>29.06.2026 16:08:42</date>
//
// <summary>
// Konsolen Applikation mit Menü
// </summary>
//-----------------------------------------------------------------------

namespace Console.DataSync
{
    using Console.DataSync.Features.Models;
    using Console.DataSync.Features.Repository;
    using Console.DataSync.Features.Service;
    /* Imports from NET Framework */
    using System;

    public class Program
    {
        public Program()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.CursorVisible = false;
        }

        private static void Main(string[] args)
        {
            CMenu mainMenu = new CMenu("Sync Data Demo");
            mainMenu.AddItem("Sync Test - 1 (Daten erstellen)", MenuPoint1);
            mainMenu.AddItem("Sync Test - 2 (Repository)", MenuPoint2);
            mainMenu.AddItem("Sync Test - 3 (ChangeTracker)", MenuPoint3);
            mainMenu.AddItem("Sync Test - 4 (Export JSON)", MenuPoint4);
            mainMenu.AddItem("Sync Test - 5 (Import JSON)", MenuPoint5);
            mainMenu.AddItem("Sync Test - 6 (Sync Data)", MenuPoint6);
            mainMenu.AddItem("Sync Test - 7 (Sync Manager)", MenuPoint7);
            mainMenu.AddItem("Beenden", () => ApplicationExit());
            mainMenu.Show();
        }

        private static void ApplicationExit()
        {
            Environment.Exit(0);
        }

        private static void MenuPoint1()
        {
            Console.Clear();

            var kunde = new Kunde
            {
                Name = "Müller",
                Ort = "Mannheim"
            };

            Console.WriteLine(kunde);

            Console.WriteLine();

            Console.WriteLine($"Id             : {kunde.Id}");
            Console.WriteLine($"ChangeNumber   : {kunde.Sync.ChangeNumber}");
            Console.WriteLine($"Deleted        : {kunde.Sync.Deleted}");

            Console.Wait();
        }

        private static void MenuPoint2()
        {
            Console.Clear();

            var repo = RepositoryFactory.Create<Kunde>("PC A");

            repo.Add(new Kunde
            {
                Name = "Müller",
                Ort = "Mannheim"
            });

            repo.Add(new Kunde
            {
                Name = "Schmidt",
                Ort = "Heidelberg"
            });

            Console.WriteLine(repo.Device.DeviceName);

            Console.WriteLine();

            foreach (var kunde in repo.ActiveItems())
            {
                Console.WriteLine(kunde);
            }

            Console.Wait();
        }

        private static void MenuPoint3()
        {
            Console.Clear();

            var repository = RepositoryFactory.Create<Kunde>("PC A");

            var tracker = new ChangeTracker<Kunde>(repository);

            var kunde = tracker.Insert(new Kunde
            {
                Name = "Müller",
                Ort = "Mannheim"
            });

            tracker.Insert(new Kunde
            {
                Name = "Schmidt",
                Ort = "Heidelberg"
            });

            foreach (var item in repository)
            {
                Console.WriteLine($"{item.Name,-15}" + $"{item.Ort,-15}" + $"Change={item.Sync.ChangeNumber}");
            }

            Console.Line();
            Console.WriteLine("Kunde ändern");
            tracker.Update(kunde.Id, k =>
            {
                k.Ort = "Karlsruhe";
            });

            foreach (var item in repository.Items)
            {
                Console.WriteLine($"{item.Name,-15}" + $"{item.Ort,-15}" + $"Change={item.Sync.ChangeNumber}");
            }

            Console.Line();
            Console.WriteLine("Kunde löschen");
            tracker.Delete(kunde.Id);

            foreach (var item in repository.Items)
            {
                Console.WriteLine($"{item.Name,-15}" + $"{item.Ort,-15}" + $"Change={item.Sync.ChangeNumber}");
            }

            Console.Wait();
        }

        private static void MenuPoint4()
        {
            Console.Clear();

            var repository = RepositoryFactory.Create<Kunde>("PC A");

            var tracker = new ChangeTracker<Kunde>(repository);

            tracker.Insert(new Kunde()
            {
                Name = "Müller",
                Ort = "Mannheim"
            });

            tracker.Insert(new Kunde()
            {
                Name = "Schmidt",
                Ort = "Heidelberg"
            });

            tracker.Update(repository.Items.First().Id,
                x =>
                {
                    x.Ort = "Karlsruhe";
                });

            var exporter = new JsonExporter<Kunde>();

            var package = exporter.Export(repository, 0);

            exporter.Save(package, "Export.json");

            Console.WriteLine($"{package.Items.Count} Datensätze exportiert.");

            Console.Wait();
        }

        private static void MenuPoint5()
        {
            Console.Clear();

            var importer = new JsonImporter<Kunde>();

            var imported = importer.Load("Export.json");

            Console.WriteLine();

            Console.WriteLine($"Von Gerät : {imported.SourceDeviceId}");

            Console.WriteLine($"Datensätze: {imported.Items.Count}");

            Console.Wait();
        }

        private static void MenuPoint6()
        {
            Console.Clear();

            Console.Line();
            Console.WriteText("Test Insert");

            var repoA = RepositoryFactory.Create<Kunde>("PC A");
            var repoB = RepositoryFactory.Create<Kunde>("PC B");

            var tracker = new ChangeTracker<Kunde>(repoA);

            tracker.Insert(new Kunde()
            {
                Name = "Müller",
                Ort = "Mannheim"
            });

            var exporter = new JsonExporter<Kunde>();

            var package = exporter.Export(repoA, 0);

            var engine = new SyncEngine<Kunde>();

            engine.Synchronize(repoB, package);

            Console.WriteLine();

            Console.WriteLine($"A: {repoA.Items.Count}");
            Console.WriteLine($"B: {repoB.Items.Count}");

            foreach (var item in repoA.Items)
            {
                Console.WriteLine($"repoA: {item.Name,-15}" + $"{item.Ort,-15}" + $"Change={item.Sync.ChangeNumber}");
            }

            foreach (var item in repoB.Items)
            {
                Console.WriteLine($"repoB: {item.Name,-15}" + $"{item.Ort,-15}" + $"Change={item.Sync.ChangeNumber}");
            }

            Console.Line();
            Console.WriteText("Test Änderung");

            var kunde = repoA.Items.First();

            tracker.Update(kunde.Id,
                x =>
                {
                    x.Ort = "Karlsruhe";
                });

            package = exporter.Export(repoA, 1);

            engine.Synchronize(repoB, package);

            foreach (var item in repoA.Items)
            {
                Console.WriteLine($"repoA: {item.Name,-15}" + $"{item.Ort,-15}" + $"Change={item.Sync.ChangeNumber}");
            }

            foreach (var item in repoB.Items)
            {
                Console.WriteLine($"repoB: {item.Name,-15}" + $"{item.Ort,-15}" + $"Change={item.Sync.ChangeNumber}");
            }

            Console.Line();
            Console.WriteText("Test Löschen");

            tracker.Delete(kunde.Id);

            package = exporter.Export(repoA, 2);

            engine.Synchronize(repoB, package);

            foreach (var item in repoA.Items)
            {
                Console.WriteLine($"repoA: {item.Name,-15}" + $"{item.Ort,-15}" + $"Change={item.Sync.ChangeNumber, -10}" + $"Deleted={item.Sync.Deleted}");
            }

            foreach (var item in repoB.Items)
            {
                Console.WriteLine($"repoB: {item.Name,-15}" + $"{item.Ort,-15}" + $"Change={item.Sync.ChangeNumber, -10}" + $"Deleted={item.Sync.Deleted}");
            }

            Console.Wait();
        }

        private static void MenuPoint7()
        {
            Console.Clear();

            var repoA = RepositoryFactory.Create<Kunde>("PC A");
            var repoB = RepositoryFactory.Create<Kunde>("PC B");

            var trackerA = new ChangeTracker<Kunde>(repoA);
            var trackerB = new ChangeTracker<Kunde>(repoB);

            var syncA = new SyncManager<Kunde>(repoA);
            var syncB = new SyncManager<Kunde>(repoB);

            var kunde = trackerA.Insert(new Kunde()
            {
                Name = "Müller",
                Ort = "Mannheim"
            });

            trackerA.Insert(new Kunde()
            {
                Name = "Schmidt",
                Ort = "Heidelberg"
            });

            Console.Title("Daten Synchronisieren von A => B");
            syncA.Export("AtoB.json", repoB.Device.DeviceId);
            syncB.Import("AtoB.json");

            Console.Title("Vorhanden Daten");
            foreach (var item in repoA.Items)
            {
                Console.WriteLine($"repoA: {item.ToString()}");
            }

            foreach (var item in repoB.Items)
            {
                Console.WriteLine($"repoB: {item.ToString()}");
            }

            Console.Title("Ändern/Löschen von Daten auf PC-B");
            trackerB.Update(kunde.Id,
                x => { x.Ort = "Karlsruhe"; });

            trackerB.Insert(new Kunde()
            {
                Name = "Meier",
                Ort = "Hamburg"
            });

            trackerB.Delete(repoB.Items.Last().Id);

            Console.Title("Daten Synchronisieren von B => A");
            syncB.Export("BtoA.json", repoA.Device.DeviceId);
            syncA.Import("BtoA.json");

            Console.Title("Vorhanden Daten nach dem Synchronisieren");
            foreach (var item in repoA.Items)
            {
                Console.WriteLine($"repoA: {item.ToString()}");
            }

            foreach (var item in repoB.Items)
            {
                Console.WriteLine($"repoB: {item.ToString()}");
            }

            Console.Wait();
        }
    }
}

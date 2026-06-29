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
            CMenu unterMenu = new CMenu("Untermenü");
            unterMenu.AddItem("Untermenüpunkt 1", () => UnterMenuPoint("A"), "🖥");
            unterMenu.AddItem("Untermenüpunkt 2", () => UnterMenuPoint("B"), "🔊");

            CMenu mainMenu = new CMenu("Hauptmenü");
            mainMenu.AddItem("Auswahl Menüpunkt 1", MenuPoint1);
            mainMenu.AddSubMenu("Einstellungen", unterMenu, "⚙");
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

            Console.Wait();
        }

        private static void UnterMenuPoint(string param)
        {
            Console.Clear();

            Console.Wait(param);
        }
    }
}

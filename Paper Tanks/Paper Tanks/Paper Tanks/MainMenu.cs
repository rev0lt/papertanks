using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameStateManagement;
namespace Paper_Tanks
{ 
    class MainMenu : MenuScreen
    {
          public MainMenu(): base(String.Empty)
        { MenuEntry startGameMenuEntry = new MenuEntry("Play");
        MenuEntry exitMenuEntry = new MenuEntry("Exit");
        
         startGameMenuEntry.Selected += StartGameMenuEntrySelected;
        exitMenuEntry.Selected += OnCancel;
            // Add entries to the menu.
            MenuEntries.Add(startGameMenuEntry);
            MenuEntries.Add(exitMenuEntry);
    
    
        }
    protected override void UpdateMenuEntryLocations()
    {
        base.UpdateMenuEntryLocations();

        foreach (var entry in MenuEntries)
        {
            var position = entry.Position;

            position.Y += 60;

            entry.Position = position;
        }
    }

    void StartGameMenuEntrySelected(object sender, EventArgs e)
    {
        ScreenManager.AddScreen(new papertanks(), null);
    }
    protected  void OnCancel()
    {
        ScreenManager.Game.Exit();
    }
    }
}

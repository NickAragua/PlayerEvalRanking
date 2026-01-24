A windows desktop utility for importing and coalescing player records, as well as building teams.

How to build teams:
- Log in to the web site and export the "Grade X/Y" shared report as excel. Drag and drop this to the gray box/grid in the bottom right corner of the application. That's your "registered player list".
- Take one or more coach evals. Drag and drop them to gray box/grid in the top right corner of the application. Review the data, then click 'Merge Eval'. This will move the player records to the
top left box, the "Coalesced Player List". This can be done as many times as you have coach evals.
- If you want to play around with the weights attached to the current/previous season/assessment scores and the player division, you can do that via "settings".
- To import assessment and previous season scores from the "master player list" file that you may have gotten last season, drag and drop it to the coalesced player list and it will be integrated.
- When you are ready to start building teams, click 'create team' one or more times, then drag and drop players from the coalesced player list to the current team.
- If you want to conveniently move players between teams, click 'Team View' and drag and drop players between teams there.
- Once satisfied with your teams, click 'export'. This will export the teams into a spreadsheet similar to what we send in when building teams, but less pretty looking. Sorry.

Some notes:

Coalesced player list (top left):

Players highlighted in red have no eval data (coach or otherwise) or have been flagged otherwise

Players highlighted in orange are not currently registered with WYSA (if you have the registrants list loaded)

If you mess up and want to undo your last action, click 'Undo'; it will undo most meaningful actions.

To save your work in progress click 'Save' and pick a destination file. To restore it, click 'Load'. 

The utility autosaves after every meaningful action. The backup is called "autosave_player_rankings.json" and lives where your executable lives. It can be loaded via the 'Load' mechanism.

Saves are likely not going to be compatible between versions of this application, especially if any of the data structures change. Which they will, as this is an "early alpha as-is software".

# OneDriveBully
Have a lot of folders you want to sync to OneDrive but not enough space in your main hard drive? 

![17435910_309645286121446_697675670786975216_o](https://user-images.githubusercontent.com/20832437/113570489-597bed80-960c-11eb-89a2-ea4dd17fde82.png)


I did, I had all my photos on the D:\ drive but OneDrive was requiring to copy them to the OneDrive folder in C:\ . But my C:\ is not big enough.

OneDrive Bully was developed to solve exactly that.

It uses Windows Symbolic Links and Junctions (thanks to micahmo) to link your folders sitting outside your OneDrive folder in any hard drive or network path. OneDrive doesn't support it currently. It will simply ignore any changes to these folders.

OneDrive Bully solves the issue by triggering OneDrive to sync, including these folders on a timer you set. The best thing is it is doing it without affecting the standard OneDrive application or requiring you to login to another OneDrive client.

You can also double click the icon and force sync whenever you want.

Saved the best for last, OneDrive is Free!

Bully your OneDrive!

Track OneDrive Bully news and updates also on Facebook: https://www.facebook.com/pg/OneDriveBully

Released Application can be downloaded from here: https://github.com/ktheod/OneDriveBully/releases


Please note the following:
----------------------------
ODB does only two things...1 provides a GUI for symlinks and settings, 2 periodically creates/renames an empty file in the root folder of OneDrive. That's it...the empty file just tricks OneDrive to rescan and sync all changes even on symbolic folders. You could have the same result if you created the symlinks by using command prompt and every 10 minutes you created manually a file in OneDrive root folder. This app just puts everything together. :)

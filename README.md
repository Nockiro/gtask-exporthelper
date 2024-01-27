# GTask Export Helper

## What is it?

In short, this utility can be used to take an export of Googles Tasks lists (via [Google Takeout as JSON](https://takeout.google.com/settings/takeout)) and use them elsewhere through the iCalendar file format.  

At the moment, there are two possible output possibilities:

- Save tasks lists as separate files in the iCalendar file format
- Save tasks lists directly via CalDAV to an already existing CalDAV server (e.g. NextCloud)

## How to use it
Since this is (at the moment) a command line program, you can set input and output via command line parameters.  
*If someone needs a graphical user interface, just ask - since I didn't need it yet, there isn't one at the moment.*

Basically, at the moment, you have one possibility to input data and two modes to output them.  
You set the location of the input file via `-j` and then either 
 * use mode `File` and give an `-o`utput path to save the generated ICS files to
 * use mode `CalDAV` and the URL/credentials for uploading to a CalDAV server   
   Note: This mode creates new lists or attempts to use already existing lists with the same name

### Examples

* Import a json file named "tasks.json" and save all lists to the folder "D:\tasks"
```bat
GTI.Cli.exe -j tasks.json --outputMode File --outputPath "D:\tasks"
```

* Import a json file named "tasks.json" and save all lists via CalDAV on `https://yournextcloud.de/remote.php/dav/calendars/youruser`
  * *Example for NextCloud*
  ```
  GTI.Cli.exe -j tasks.json --outputMode CalDAV --calDavUri https://yournextcloud.de/remote.php/dav/calendars/youruser --calDavUser youruser --calDavPass G00dAppP@ssw0rd
  ``` 
  * *Example for Baikal*
  ```
  GTI.Cli.exe -j tasks.json --outputMode CalDAV --calDavUri https://baikal.yourserver.de/dav.php/calendars/youruser --calDavUser youruser --calDavPass G00dAppP@ssw0rd
  ``` 

### Complete parameter list

```
  -j, --jsonInput     [Required.] Path to Google Task JSON export file

  -m, --outputMode    (Default: File) Mode that the output generation should follow.
                      Must be one of the following: File, CalDAV
                      Note: CalDAV mode currently only works with adding new calendars.

  -o, --outputPath    Path to the folder in which the files generated are to be saved.
                      OutputMode: File

  --calDavUri         Base URL of the CALDAV endpoint.
                      OutputMode: CalDAV
                      Example: https://yournextcloud.tld/remote.php/dav/calendars/youruser

  --calDavUser        User for the CALDAV endpoint.
                      OutputMode: CalDAV

  --calDavPass        Password for the CALDAV endpoint.
                      OutputMode: CalDAV

  --help              Display this help screen.

  --version           Display version information.
```


## Why is it?
As far as I know, there is at the time of writing no possibility other than the above mentioned JSON export to get data from Google Tasks to another platform.  

Since most platforms work with either text files or the iCalendar file format for synchronizing and/or importing tasks, there seems to be not a single possibility to directly access tasks without using the export or Google's Tasks API.  
To work around that issue, the tool from this repository can use the JSON export and either convert it into standardized, multi-platform-usable ICS files or directly upload the tasks via CalDAV.

### Yes, but why is there a built-in direct upload?
Originally, I wanted to just take the ICS files and upload them to my NextCloud instance.  
After two attempts failed (Nextcloud import doesn't import finished tasks, Thunderbird as end client gets stuck after two tasks), I decided, that this tool could also just take the ICS output and send them to the Nextcloud CalDAV API directly.

If only just a single person doesn't have to deal with the horror story that is trying to get Google's tasks imported somewhere else, I'm glad.

## Known limitations

Since Google's API as well as their takeout do not contain all information that is possible for tasks in the iCalendar format, this program cannot save that information.  
Known limits include
  - The repetition on recurring events is not recognized
  - All-day events sometimes seem to be recognized, sometimes not - the API doesn't contain that information, though.
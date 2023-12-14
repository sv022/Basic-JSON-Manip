# JSON-App-2
User to parse the JSON file containing information about university education programs and standarts.

The structure of the target file goes as follows:
```
{
  "authorFIO": "...",
  "educationDirectionCode": "...",
  "educationDirectionTitle": "...",
  "fgosOrderNumber": "...",
  "id": "...",
  "educationLevelTitle": "...",
  "content": {
    ...
  }
}
```
Files are read from `bin/Debug/net7.0/ProfessionalStandards.json`

Includes a basic command line interface.

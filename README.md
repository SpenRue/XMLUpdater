# XMLUpdater
Updates specific values of an XML file

## Command Line Usage

-f, --file       Required. The XML file to modify.

-x, --xpath      Required. The XPath to select.

-r, --regex      The regex to select from the xpath value.

-g, --group      (Default: 1) The group number of the of the regex to
                   replace, or -1 to replace the entire match. Not zero based

-t, --text       Required. The text to replace the selected xpath value with.

-v, --verbose    (Default: True) Print details during execution.

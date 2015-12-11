# sd-msmqooz 

## Introduction

A server density plugin for monitoring msmq health that works in windows.

### **Metrics**

**total_message_count**: Count of messages in all queues.  
**[queue_name]**:  Count of message of the user-defined queue.

> You can define as many queues to monitor, and each of these queues will be shown on the dashboard as part of the metrics results

----------


## Installation

> Server Density Agent Service on windows is usually installed in this directory **"C:\Program Files (x86)\Boxed Ice\Server Density\"**. You can read the [documentation](https://support.serverdensity.com/hc/en-us/articles/201253673-Writing-a-plugin-Windows) here.

 - go to directory **"C:\Program Files (x86)\Boxed Ice\Server Density\"** (your server density agent directory)
 - edit the **"BoxedIce.ServerDensity.Agent.WindowsService.exe.config"** and add comma separated values of your queue names such as:

      <appSettings>
        <add key="Msmqooz-Queues" value="myQueueOne,myQueueTwo" />
      </appSettings>

 - download plugin zip **https://github.com/khalilovcmd/sd-msmqooz/releases/download/0.0.1/Msmqooz.zip**
 - create a new folder and name it as **"plugins"**
 - copy the downloaded files into the **"plugins"** folder
 - "Open Server Density by locating it in your start menu. Click the Enable plugins check box and then click the Browse... button. Choose the folder that you created that contains your plugin DLLs. The agent will parse this directory for any new monitoring plugin DLLs each time the agent is started." - excerpt from [documentation](https://support.serverdensity.com/hc/en-us/articles/201253673-Writing-a-plugin-Windows)
 - restart the Server Density Agent service

----------

## Usage

TODO: Write usage instructions

----------

## Contributing

1. Fork it!
2. Create your feature branch: `git checkout -b my-new-feature`
3. Commit your changes: `git commit -am 'Add some feature'`
4. Push to the branch: `git push origin my-new-feature`
5. Submit a pull request :D

----------

## History

TODO: Write history





=======



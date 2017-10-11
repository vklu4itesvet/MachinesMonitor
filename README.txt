There are 2 apps:
1. MachinesParkEmulator console, which sends pings for random subset of vehicles once per 1 minute. Vin codes of vehicles, that were not pinged during 
   predefined in config timeout (For avoiding noice status change when only 1 of n ping attempts failed, timeout can be set greater than 1 min)
   are displayed in console for easy understanding, what vehicles should be considered as disconnected by monitoring web app.
2. MachinesMonitor web app, that rechecks inmemory cache with statuses and ping times for each vehicle once per 1 minute and invalidates status if no ping 
   from vehicle was received during 2 mins. Info is can be observed as json by key "isConnected" at default rout by refreshing it after ping from console
   is sent. 

Info about status is stored in non blocking collection in memory and is persisted on shutdown in order to allow quick app restarts with abbility to
display actual data on startup.
Initial idea was to generate db via codefirst approach, fullfill it with data and deliver solution with mdf file. It can be easyly done with
full framework platform, however many issues occured with Core. Finally static class that generates entities was created in order not to spend too much time.

Several data grids for Angular where tried for displayed data, however issues with styles took much time and finally front-end was posponed. If, back-end code
is ok but front-end skills evaluation is required - front-end can be implemented, just need more time.








# Essential Layers
### EssentialLayers.SQLite

Is a complement to the package [EssentialLayers](/EssentialLayers/Readme.md) to provide an extra layer to use SQLite with the EssentialLayers standard.

#### If you want to use:
You need to add a nes Instance in your **Program.cs** file.

```
services.AddSQLiteInstance(
	DB_CLIENT,
	databasePathFactory: provider =>
	{
		IDeviceInfoService deviceInfo = provider.GetRequiredService<IDeviceInfoService>();

		return Path.Combine(deviceInfo.Path, $"{DB_CLIENT}.db3");
	}
);
```

#### Release Notes

##### v1.4.0
 - feat: Support to async database and query services `15-04-2026`
 - refactor: New extensions asbtraction of ILogger `15-04-2026`

##### v1.3.0
 - feat: Query Support in query database service by query string and optional parameters `10-04-2026`
 - feat: Execute Support in database service by script and optional parameters `10-04-2026`

##### v1.2.0
 - feat: Re-open connection on Reset database `07-04-2026`

##### v1.1.0
 - feat: Log and Error messages + New version property in both interfaces `10-03-2026`

Created by [Mario Soto Moreno](https://github.com/msotodev)
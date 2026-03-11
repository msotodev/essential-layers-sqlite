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
 - feat: Log and Error messages + New version property in both interfaces `10-03-2026`

Created by [Mario Soto Moreno](https://github.com/msotodev)
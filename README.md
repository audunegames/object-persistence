# Audune Persistence

Store persistent data from your components on disk or in the cloud with an easily configurable persistence system.

See the [wiki](https://github.com/audunegames/persistence/wiki) of the repository to get started with the package.

## Features

* A persistence system component that is configured solely with components on a GameObject. Add adapters to the system to specify filesystems or cloud services to save and load data. Acces the functonality of the system through scripting.
* Save and load the state of a component using interfaces to mark them as serializable, deserializable, or both. Craft a state structure reminiscent of JSON to persist only the data you want to persist.
* List over files in the adapters and move, copy, or delete them through scripting.
* Define custom backends to define how the state is stored in a file. The persistene system uses [MessagePack](https://msgpack.org/) to serialize states by default, but this can easily be changed on the component.
* Define custom adapters for different filesystems or cloud services. A local filesystem adapter is included.

## Installation

## Requirements

This package depends on the following packages:

* [UnityEditor Utilities](https://openupm.com/packages/com.audune.utils.unityeditor/), version **1.0.1** or higher.
* [MessagePack](https://openupm.com/packages/net.tnrd.messagepack/), version **2.5.125** or higher.

If you're installing the required packages from the [OpenUPM registry](https://openupm.com/), make sure to add a scoped registry with the URL `https://package.openupm.com` and the required scopes before installing the packages.

## Installing from the OpenUPM registry

To install this package as a package from the OpenUPM registry in the Unity Editor, use the following steps:

* In the Unity editor, navigate to **Edit › Project Settings... › Package Manager**.
* Add the following Scoped Registry, or edit the existing OpenUPM entry to include the new Scope:

```
Name:     package.openupm.com
URL:      https://package.openupm.com
Scope(s): com.audune.persistence
```

* Navigate to **Window › Package Manager**.
* Click the **+** icon and click **Add package by name...**
* Enter the following name in the corresponding field and click **Add**:

```
com.audune.utils.unityeditor
```

## Installing as a Git package

To install this package as a Git package in the Unity Editor, use the following steps:

* In the Unity editor, navigate to **Window › Package Manager**.
* Click the **+** icon and click **Add package from git URL...**
* Enter the following URL in the URL field and click **Add**:

```
https://github.com/audunegames/persistence.git
```


## License

This package is licensed under the GNU GPL 3.0 license. See `LICENSE.txt` for more information.

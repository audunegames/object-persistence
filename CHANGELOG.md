# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [1.0.1] - 2024-12-27

### Changed

- Updated source code license from GPL 3.0 to LGPL 3.0, so that larger works can use the library without disclosing their source code.

## [1.0.0] - 2024-12-10

### Added

- The persistence system can now be accessed using a static instance field.

### Changed

- Updated code to use the separate Object Serialization package.
- Changed file reference to show a dropdown for the adapter name.

## [0.3.1] - 2024-06-15

### Added

- Added enum methods to the state class.

### Changed

- Updated equality methods of states.

## [0.3.0] - 2024-06-15

### Added

- Added default values to list and object state getters.
- Added list and object state interfaces for acessing properties of vectors and quaternions through paths.

### Changed

- Improved serialization extensions code.

## [0.2.5] - 2024-04-11

### Changed

- Updated code to use the separate Serialized Types package.

## [0.2.4] - 2024-03-20

### Changed

- Refactored package structure.

## [0.2.3] - 2024-02-29

### Changed

- Read functions now take an IDeserializable instance instead of a generic object.
- MessageStateFormatter is now an internal class.

## [0.2.2] - 2024-02-24

### Added

- Components now have icons in the inspector.

### Changed

- Refactored package structure.

## [0.2.1] - 2024-02-22

### Added

- Custom editor for the persistence system.

## [0.2.0] - 2024-02-22

### Added

- The persistence system now uses adapters for more options to save data to.

## [0.1.0] - 2024-01-24

### Added

- Persistence system that saves and loads data from the local file system.
- Classes for serializing and deserializing C# value types and strings.
- Interfaces to declare a custom object to be serializable.
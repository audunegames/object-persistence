# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [0.2.4] - 2024-02-29

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
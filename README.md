# ArchivableUserData

Trying out loading Rhino UserData without knowing class definition at compilation.

`DynLoadTestClass` is serialized with its assembly on serialization and is dynamic reinstated on deserialization (assuming running on a different system and without the class definition loaded into the assembly).

## Test
Project is setup as a Rhino plugin.  
Build and install the plugin.  
Run `LoadData`. This will serialize and load instances of `DynLoadTestClass` into the test object.  
Save the file.  
Delete the class file for `DynLoadTestClass` and build again.  
Run `UnoadData`. This will read file assembly for `DynLoadTestClass` deserialized from the object and dynamically load its instances.  

## Issues
- Loading referenced assemblies for dynamic compilation.  
- Reading stored custom UserData without knowing type.  
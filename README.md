# ArchivableUserData

Trying out loading Rhino UserData without knowing class definition on compilation.

`DynLoadTestClass` is serialized with its assembly on serialization and is dynamically reinstated on deserialization (assuming running on a different system and without the class definition loaded into the assembly).

## Test
Project is setup as a Rhino plugin.  
Build and install the plugin.  
Run `LoadData`. This will serialize and load instances of `DynLoadTestClass` into the test object.  
Save and close file.  
Delete the class file for `DynLoadTestClass` and build again.  
Run `ReadData`. This will read the file assembly for `DynLoadTestClass` from the object and dynamically load its instances.  

## Issues
- Loading referenced assemblies during dynamic compilation.  
- Reading stored custom UserData without knowing exact type.  
## adduser command

### Summary

You can add the user's name whenever you need, with the password if required.

### Description

If you need to add a person that wants to use the kernel, you can add users for them, and let them specify the password if they need. This way, adduser will only create an account and gets the permissions for the new user ready, and the new user will be a normal account for security reasons.

However if you need to add a person that has admin rights, you should set the permission for the user to allow admin rights. If you want to temporarily disable an account so it blocks the log-on request to that account, you should set the disabled permission to Enabled.

### Command usage

* `adduser <userName>`
* `adduser <userName> [password] [confirm]`

### Examples

* `adduser joe`: This will create a new account with the name of "joe"
* `adduser test pass1234 pass1234`: This will create a new account with the name of "test" and the password of "pass1234"
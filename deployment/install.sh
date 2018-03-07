#!/bin/bash

# to be run locally on the server in order to upgrade the bot

#remove the old backup
sudo rm -r -f /var/eliteIngress_old

#create a backup of the current version
sudo mv /var/eliteIngress /var/eliteIngress_old

# we need an installation path
sudo mkdir /var/eliteIngress

# copy the new files
sudo mv /tmp/eliteIngress_publish/* /var/eliteIngress

# lock down perms
sudo chgrp www-data /var/eliteIngress
sudo chmod 0755 /var/eliteIngress

# restart the service
sudo supervisorctl restart eliteIngress

# add new user into sudo group
sudo adduser kafka-user
sudo usermod -aG sudo kafka-user


sudo add-apt-repository -y ppa:webupd8team/java
sudo apt-get update
sudo apt-get install oracle-java8-installer -y
sudo apt-get install zookeeperd
wget http://www-eu.apache.org/dist/kafka/1.0.1/kafka_2.11-1.0.1.tgz

sudo mkdir /opt/Kafka
sudo tar -xvf kafka_2.11-1.0.1.tgz -C /opt/Kafka/

cd /opt/Kafka/kafka_2.11-1.0.1/

sudo ./bin/kafka-server-start.sh ./config/server.properties
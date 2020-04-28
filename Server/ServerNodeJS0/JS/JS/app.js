'use strict';

var net = require('net');
var clients = [];
var server = net.createServer(function (socket) {
    //socket.setKeepAlive(true);
    //socket.setKeepAlive(true, 1000);
    /*
    socket.setTimeout(10000, function () {
        console.log(socket.address().address + " timeout.");
        socket.end('idle time, disconnecting..');
    });
    */

    console.log(socket.address().address + " connected.");
    clients.push(socket);
    socket.on('data', function (data) {
        console.log(data);
        socket.write(data);
        var sender = this;
        clients.forEach(function (client) {
            if (client !== sender) client.write(data);
        });
    });

    socket.on('end', function () {
        console.log('client disconnted. end');
    });

    socket.on('close', function () {
        console.log('client disconnted. close 에러나도 실행');
        clients.splice(clients.indexOf(socket), 1);
    });

    socket.on('error', function (err) {
        console.log(err);
    });
});

server.listen(4000, function () {
    console.log('linsteing on 4000..');
});
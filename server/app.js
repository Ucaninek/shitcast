const fs = require('fs');
const express = require('express');
const chalk = require('chalk');
var bodyparser = require('body-parser');

class Message {
    constructor(message, author) {
        this.author = author;
        let currentDate = new Date();
        let h = currentDate.getHours();
        let m = currentDate.getMinutes();
        if (h.length == 1) h = "0" + h;
        if (m.length == 1) m = "0" + m;
        let time = h + ":" + m;
        this.timestamp = time;
        this.message = message;
    }
}

class User {
    constructor(username) {
        this.name = username;
    }
}

const app = express();

var db = JSON.parse(fs.readFileSync('./database.json'));
var messages = [];
var users = [];

function updateDB() {
    fs.writeFileSync('./database.json', JSON.stringify(db));
}

const PORT = db.port;

app.use(bodyparser.urlencoded({ extended: false }));
app.use(bodyparser.json());

//GET /api/users
app.get('/api/users', (_req, res) => {
    return res.json(users);
});

//GET /api/messages
app.get('/api/messages', (_req, res) => {
    return res.json(messages);
});

//POST /api/messages
app.post('/api/messages', function (req, res) {
    var message = req.body.message;
    var author = req.body.author;
    messages.push(new Message(message, author));
    console.log(chalk.red('=> ') + chalk.whiteBright(author) + ': ' + message);
    return res.end('message added.');
});

//POST /api/users
app.post('/api/users', function (req, res) {
    var user = req.body.user;
    if (users.find(iUser => iUser.name == user) != undefined) return res.end('1'); //user already exists.
    users.push(new User(user));
    return res.end('User added.');
});

app.get('/', function (_req, res) {
    res.status(200);
    return res.end('shitcast');
})

app.listen(PORT, function () {
    console.log(chalk.bgRed('-- SHITCAST SERVER --'));
    console.log(`${chalk.whiteBright('Server listening')} on port ${chalk.red(PORT)}`);
    console.log(`${chalk.whiteBright('Server ip')} is ${chalk.red(`${getIpv4Addr()}${chalk.grey(':')}${PORT}`)} => share this with your friends`)
});

function getIpv4Addr() {
    const { networkInterfaces } = require('os');

    const nets = networkInterfaces();
    const results = Object.create(null); // Or just '{}', an empty object

    for (const name of Object.keys(nets)) {
        for (const net of nets[name]) {
            // Skip over non-IPv4 and internal (i.e. 127.0.0.1) addresses
            // 'IPv4' is in Node <= 17, from 18 it's a number 4 or 6
            const familyV4Value = typeof net.family === 'string' ? 'IPv4' : 4
            if (net.family === familyV4Value && !net.internal) {
                if (!results[name]) {
                    results[name] = [];
                }
                results[name].push(net.address);
            }
        }
    }

    return results[Object.keys(results)[0]];
}
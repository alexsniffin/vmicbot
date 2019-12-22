const eris = require('eris');
const axios = require('axios');

process.env['NODE_TLS_REJECT_UNAUTHORIZED'] = '0';

const bot = new eris.Client('');
const host = "localhost:5000"

bot.on('ready', () => {
   console.log('Connected and ready.');
});

bot.on('messageCreate', async (msg) => {
    try {
        if (msg.content.startsWith("!vm play ", 0)) {
            var message = msg.content.split("!vm play ")
            var request = 'http://' + host + '/api/audio/play?link=' + message[1]
            msg.channel.createMessage('Attempting to get audio...');
            await axios.get(request).then((response) => {
                msg.channel.createMessage('Playing!');
            })
            .catch(error => {
                msg.channel.createMessage('Something went wrong...');
                console.log(error);
            });
        } else if (msg.content === "!vm stop") {
            var request = 'http://' + host + '/api/audio/stop'
            await axios.get(request).then((response) => {
                msg.channel.createMessage('Stopped playing!');
            })
            .catch(error => {
                msg.channel.createMessage('Hmmm, some reason this didn\'t work right...');
                console.log(error);
            });
        } else if (msg.content === "!vm pause") {
            var request = 'http://' + host + '/api/audio/pause'
            await axios.get(request).then((response) => {
                msg.channel.createMessage('Pausing audio');
            })
            .catch(error => {
                msg.channel.createMessage('Hmmm, some reason this didn\'t work right...');
                console.log(error);
            });
        } else if (msg.content === "!vm resume") {
            var request = 'http://' + host + '/api/audio/resume'
            await axios.get(request).then((response) => {
                msg.channel.createMessage('Resuming audio');
            })
            .catch(error => {
                msg.channel.createMessage('Hmmm, some reason this didn\'t work right...');
                console.log(error);
            });
        }
    } catch (err) {
        console.warn('Failed to respond to mention.');
        console.warn(err);
    }
});

bot.on('error', err => {
   console.warn(err);
});

bot.connect();
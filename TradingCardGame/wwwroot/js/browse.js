const channelsElement = document.querySelector('div.channels');
const searchButton = document.getElementById('searchButton');
const searchInput = document.getElementById('searchInput');

searchButton.addEventListener('click', () => {
    const name = searchInput.value;
    if (!name) {
        return;
    }

    renderChannels(name);
});

searchInput.addEventListener('input', () => {
    const name = searchInput.value;
    if (!name) {
        return;
    }

    renderChannels(name);
});

async function renderChannels(name) {
    const data = await getChannels(name);
    channelsElement.innerHTML = '';

    console.log(data);

    if (data.length === 0) {
        const noChannelsElement = document.createElement('p');
        noChannelsElement.textContent = 'No channels found :(';
        noChannelsElement.classList.add('not-found')
        channelsElement.appendChild(noChannelsElement);
        channelsElement.classList.add('no-channels');
        return;
    } else {
        channelsElement.classList.remove('no-channels');
    }

    let counter = 1;
    for (const channel of data) {

        const channelNumberElement = document.createElement('div');
        channelNumberElement.textContent = counter++;
        channelNumberElement.classList.add('channel-number');

        const channelNameElement = document.createElement('div');
        channelNameElement.classList.add('channel-name');
        channelNameElement.textContent = channel.name;

        const channelPlayerElement = document.createElement('div');
        channelPlayerElement.classList.add('channel-players');
        channelPlayerElement.textContent = channel.currentPlayers + ' / ' + channel.maxPlayers;

        const channelStatusElement = document.createElement('div');
        channelStatusElement.classList.add('channel-status');

        const channelElement = document.createElement('div');
        channelElement.classList.add('channel');
        let buttonElement;
        if (channel.status === 1) {
            buttonElement = document.createElement('a');
            buttonElement.classList.add('btn', 'btn-primary', 'text-white', 'w-75');
            buttonElement.href = `/Browse/JoinChannel?channelId=${channel.id}`;
            buttonElement.textContent = 'Join';
        } else if (channel.status === 2) {
            buttonElement = document.createElement('button');
            buttonElement.setAttribute('disabled', true);
            buttonElement.classList.add('btn', 'btn-warning', 'text-white', 'w-75');
            buttonElement.textContent = 'Already joined';
        } else if (channel.status === 3) {
            buttonElement = document.createElement('button');
            buttonElement.setAttribute('disabled', true);
            buttonElement.classList.add('btn', 'btn-danger', 'text-white', 'w-75');
            buttonElement.textContent = 'Channel is private';
        }

        channelStatusElement.appendChild(buttonElement);

        channelElement.appendChild(channelNumberElement);
        channelElement.appendChild(channelNameElement);
        channelElement.appendChild(channelPlayerElement);
        channelElement.appendChild(channelStatusElement);

        channelsElement.appendChild(channelElement);
    }
}

function getChannels(name) {
    return fetch("/Browse/GetChannelsContainingName?name=" + name)
        .then(response => response.json())
        .then(data => data);
}
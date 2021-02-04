const allChannels = Array.from(document.querySelectorAll("div.single-channel"));

function clearChannels() {
    const selectedChannel = getSelectedChannel();
    if (!selectedChannel) {
        return;
    }

    let text = selectedChannel.children[0].textContent.substr(2);
    selectedChannel.children[0].textContent = text;
    selectedChannel.classList.remove('active');
}

function getSelectedChannel() {
    for (const channel of allChannels) {
        if (channel.classList.contains("active")) {
            return channel;
        }
    }
}

allChannels.map(channel => {
    channel.addEventListener('click', () => {
        clearChannels();
        channel.classList.add('active');
        channel.children[0].textContent = `> ${channel.children[0].textContent}`;
        getChannelContent();
    });
});

window.onload = function () {
    const userChannels = document.querySelector('div.user-channels');
    userChannels.children[0].click();
}

function getChannelContent() {
    const selectedChannel = getSelectedChannel();
    const channelName = selectedChannel.children[0].textContent.substr(2);
    fetch(`/Channel/GetChannelContent?channelName=${channelName}`)
        .then(response => response.json())
        .then(data => console.log(data));
}
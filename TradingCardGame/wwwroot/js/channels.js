const allChannels = Array.from(document.querySelectorAll("div.single-channel"));

function clearChannels() {
    allChannels.forEach(channel => {
        if (channel.classList.contains('active')) {
            let text = channel.children[0].textContent.substr(2);
            channel.children[0].textContent = text;
            channel.classList.remove('active');
        }
    });
}

allChannels.map(channel => {
    channel.addEventListener('click', () => {
        clearChannels();
        channel.classList.add('active');
        channel.children[0].textContent = `> ${channel.children[0].textContent}`;

        // make request to the server
    });
});

window.onload = function () {
    const userChannels = document.querySelector('div.user-channels');
    userChannels.children[0].click();
}
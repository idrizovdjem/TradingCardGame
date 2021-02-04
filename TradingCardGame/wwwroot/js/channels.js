const allChannels = Array.from(document.querySelectorAll("div.single-channel"));
const errorAlert = document.getElementById('errorAlert');
errorAlert.style.display = 'none';

allChannels.map(channel => {
    channel.addEventListener('click', async () => {
        clearChannels();
        channel.classList.add('active');
        channel.children[0].textContent = `> ${channel.children[0].textContent}`;
        let { name, posts } = await getChannelContent();
        renderChannelContent(name, posts);
    });
});

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

function renderChannelContent(name, posts) {
    const headerElement = document.querySelector('h1.header');
    headerElement.textContent = name;

    const channelPostsElement = document.querySelector('div.channel-posts');

    if (posts.length === 0) {
        const noPostsElement = document.createElement('p');
        noPostsElement.textContent = 'There are no posts. Be the first to light the fire ;)';
        noPostsElement.classList.add('no-posts');
        channelPostsElement.appendChild(noPostsElement);
        return;
    }

    for (const post of posts) {
        const idInput = document.createElement('input');
        idInput.setAttribute('type', "hidden");
        idInput.value = post.id;

        const creatorElement = document.createElement('span');
        creatorElement.classList.add('post-creator');
        creatorElement.textContent = `Creator: ${post.creator}`;

        const contentElement = document.createElement('p');
        contentElement.textContent = post.content;

        const dateElement = document.createElement('span');
        dateElement.classList.add('post-date');
        dateElement.textContent = `Created on: ${post.createdOn}`;

        const postScoreElement = document.createElement('div');
        postScoreElement.classList.add('post-score');

        const scoreElement = document.createElement('span');
        scoreElement.textContent = `Score: ${post.score}`;

        const likeElement = document.createElement('i');
        likeElement.classList.add('far', 'fa-thumbs-up', 'fa-lg', 'fa-fw', 'like-icon');

        const dislikeElement = document.createElement('i');
        dislikeElement.classList.add('far', 'fa-thumbs-down', 'fa-lg', 'fa-fw', 'dislike-icon');

        scoreElement.appendChild(likeElement);
        scoreElement.appendChild(dislikeElement);
        postScoreElement.appendChild(scoreElement);

        const postElement = document.createElement('div');
        postElement.classList.add('post');
        postElement.appendChild(idInput);
        postElement.appendChild(creatorElement);
        postElement.appendChild(contentElement);
        postElement.appendChild(dateElement);
        postElement.appendChild(postScoreElement);

        channelPostsElement.appendChild(postElement);
    }
}

function getChannelContent() {
    const selectedChannel = getSelectedChannel();
    const channelName = selectedChannel.children[0].textContent.substr(2);
    return fetch(`/Channel/GetChannelContent?channelName=${channelName}`)
        .then(response => response.json())
        .then(data => data);
}

function showTextArea(event) {
    event.target.style.display = 'none';
    const postContentArea = document.getElementById('postContentArea');
    postContentArea.style.display = 'block';
    const textArea = postContentArea.children[0].children[0];
    textArea.focus();
}

async function createPost(event) {
    event.preventDefault();

    const postContentArea = document.getElementById('postContentArea');
    const textArea = postContentArea.children[0].children[0];

    const content = textArea.value.trim();
    if (content.length < 3) {
        errorAlert.style.display = 'block';
        setTimeout(() => {
            errorAlert.style.display = 'none';
        }, 2000);
        return;
    }

    const token = document.querySelector('input[name="__RequestVerificationToken"]').value;
    const channelName = getSelectedChannel().children[0].textContent.substr(2);

    let data = await fetch(`/Post/Create`, {
        method: 'POST',
        mode: 'cors',
        headers: {
            'RequestVerificationToken': token,
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({ content, channelName })
    })
        .then(response => response.json())
        .then(data => data);


    const noPostsElement = document.querySelector('p.no-posts');
    if (noPostsElement) {
        noPostsElement.remove();
    }

    const channelPosts = document.querySelector('div.channel-posts');
    channelPosts.innerHTML = `<div class="post">${data.content}</div>` + channelPosts.innerHTML;

    const dummyInput = document.getElementById('dummyInput');
    dummyInput.style.display = 'block';
    postContentArea.style.display = 'none';
    textArea.value = '';
}

function likePost(event) {

}

function dislikePost(event) {

}

window.onload = function () {
    const userChannels = document.querySelector('div.user-channels');
    userChannels.children[0].click();
}
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
    const cardsElement = document.createElement('a');
    cardsElement.classList.add('channel-nav-item');
    cardsElement.textContent = 'Cards';

    const informationElement = document.createElement('a');
    informationElement.classList.add('channel-nav-item');
    informationElement.textContent = 'Information';
    informationElement.setAttribute('href', '/Manage/Information?channelName=' + name);

    const manageElement = document.createElement('a');
    manageElement.classList.add('channel-nav-item');
    manageElement.textContent = 'Manage';
    manageElement.setAttribute('href', '/Manage/Index?channelName=' + name);

    const leaveElement = document.createElement('a');
    leaveElement.classList.add('channel-nav-item');
    leaveElement.textContent = 'Leave';
    leaveElement.setAttribute('href', '/Manage/Leave?channelName=' + name);

    const channelNavElement = document.getElementById('channelNav');
    channelNavElement.innerHTML = '';
    channelNavElement.classList.add('channel-nav');
    channelNavElement.appendChild(cardsElement);
    channelNavElement.appendChild(informationElement);
    channelNavElement.appendChild(manageElement);
    channelNavElement.appendChild(leaveElement);

    const headerElement = document.querySelector('h1.header');
    headerElement.textContent = name;

    const channelPostsElement = document.querySelector('div.channel-posts');
    channelPostsElement.innerHTML = '';

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

        const scoreSpanElement = document.createElement('span');

        const scoreElement = document.createElement('span');
        scoreElement.textContent = `Score: ${post.score}`;

        const likeElement = document.createElement('img');
        let picture = post.isVoted ? 'redHeart.png' : 'heart.png';
        likeElement.setAttribute('src', `../icons/${picture}`);
        likeElement.classList.add('heart-button');
        likeElement.addEventListener('click', () => {
            likePost(post, scoreElement, likeElement)
        });

        scoreSpanElement.appendChild(scoreElement);
        scoreSpanElement.appendChild(likeElement);
        postScoreElement.appendChild(scoreSpanElement);

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

    const idInput = document.createElement('input');
    idInput.setAttribute('type', "hidden");
    idInput.value = data.id;

    const creatorElement = document.createElement('span');
    creatorElement.classList.add('post-creator');
    creatorElement.textContent = `Creator: ${data.creator}`;

    const contentElement = document.createElement('p');
    contentElement.textContent = data.content;

    const dateElement = document.createElement('span');
    dateElement.classList.add('post-date');
    dateElement.textContent = `Created on: ${data.createdOn}`;

    const postScoreElement = document.createElement('div');
    postScoreElement.classList.add('post-score');

    const scoreSpanElement = document.createElement('span');

    const scoreElement = document.createElement('span');
    scoreElement.textContent = `Score: ${data.score}`;

    const likeElement = document.createElement('img');
    let picture = data.isVoted ? 'redHeart.png' : 'heart.png';
    likeElement.setAttribute('src', `../icons/${picture}`);
    likeElement.classList.add('heart-button');
    likeElement.addEventListener('click', () => {
        likePost(data, scoreElement, likeElement)
    });

    scoreSpanElement.appendChild(scoreElement);
    scoreSpanElement.appendChild(likeElement);
    postScoreElement.appendChild(scoreSpanElement);

    const postElement = document.createElement('div');
    postElement.classList.add('post');
    postElement.appendChild(idInput);
    postElement.appendChild(creatorElement);
    postElement.appendChild(contentElement);
    postElement.appendChild(dateElement);
    postElement.appendChild(postScoreElement);

    channelPosts.insertBefore(postElement, channelPosts.firstChild);

    const dummyInput = document.getElementById('dummyInput');
    dummyInput.style.display = 'block';
    postContentArea.style.display = 'none';
    textArea.value = '';
}

function likePost(post, scoreElement, likeElement) {
    let votes;
    let picture = likeElement.getAttribute('src').includes('heart');
    if (picture) {
        votes = Number(scoreElement.textContent.substr(6)) + 1;
        picture = 'redHeart.png';
    } else {
        votes = Number(scoreElement.textContent.substr(6)) - 1;
        picture = 'heart.png';
    }

    fetch(`/Post/Vote?postId=${post.id}`);

    likeElement.setAttribute('src', `../icons/${picture}`);
    scoreElement.textContent = `Score: ${votes}`;
}

window.onload = function () {
    const userChannels = document.querySelector('div.user-channels');
    userChannels.children[1].click();
}
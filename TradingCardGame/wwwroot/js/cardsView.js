const cards = Array.from(document.querySelectorAll('div.small-card'));
const cardPreview = document.querySelector('div.card-preview');
cards.map(card => {
    card.addEventListener('click', (event) => {
        const cardImage = card.children[0].src;
        const cardName = event.currentTarget.children[1].value;
        const cardDescription = event.currentTarget.children[2].value;
        const attack = event.currentTarget.children[3].value;
        const defense = event.currentTarget.children[4].value;
        const cardType = event.currentTarget.children[5].value;
        const cardId = event.currentTarget.children[6].value;

        cardPreview.children[1].children[0].src = cardImage;
        cardPreview.children[0].children[0].textContent = cardName;
        cardPreview.children[2].children[0].innerHTML = `[${cardType}]<br>${cardDescription}`;
        cardPreview.children[3].textContent = `ATT: ${attack} | DEF: ${defense}`;
        document.getElementById('editCard').href = '/Card/Edit?cardId=' + cardId;
    });
});

window.onload = async function () {
    const channelName = sessionStorage.getItem('selectedChannel');
    document.getElementById('deckLink').href += '?channelName=' + channelName;

    const userRole = await fetch('/Channel/GetUserRole?channelName=' + channelName)
        .then(response => response.json())
        .then(data => data);

    if (userRole === 'Administrator' || userRole === 'Moderator') {
        renderCardsForReview(channelName);
        renderArchivedCards(channelName);
    }
}

async function renderCardsForReview(channelName) {
    const cards = await getCardsForReview(channelName, 'ForReview');
    const reviewSection = document.getElementById('reviewSection');
    for (const card of cards) {

        const imgElement = document.createElement('img');
        imgElement.src = card.image;

        const cardContainer = document.createElement('div');
        cardContainer.classList.add('small-card');
        cardContainer.appendChild(imgElement);

        cardContainer.addEventListener('click', () => {
            window.location.href = '/Card/Review?cardId=' + card.id;
        });

        reviewSection.appendChild(cardContainer);
    }
}

async function renderArchivedCards(channelName) {
    const cards = await getCardsForReview(channelName, 'Archived');
    const archivedSection = document.getElementById('archivedSection');
    for (const card of cards) {

        const imgElement = document.createElement('img');
        imgElement.src = card.image;

        const cardContainer = document.createElement('div');
        cardContainer.classList.add('small-card');
        cardContainer.appendChild(imgElement);

        cardContainer.addEventListener('click', () => {
            window.location.href = '/Card/Review?cardId=' + card.id;
        });

        archivedSection.appendChild(cardContainer);
    }
}

async function getCardsForReview(channelName, status) {
    return await fetch(`/Card/GetCardsWithStatus?channelName=${channelName}&status=${status}`)
        .then(response => response.json())
        .then(data => data);
}
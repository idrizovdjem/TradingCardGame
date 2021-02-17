const cards = Array.from(document.querySelectorAll('div.small-card'));
const cardPreview = document.querySelector('div.card-preview');
cards.map(card => {
    card.addEventListener('mouseover', (event) => {
        const cardImage = card.children[0].src;
        const cardName = event.currentTarget.children[1].value;
        const cardDescription = event.currentTarget.children[2].value;
        const attack = event.currentTarget.children[3].value;
        const defense = event.currentTarget.children[4].value;
        const cardType = event.currentTarget.children[5].value;

        cardPreview.children[1].children[0].src = cardImage;
        cardPreview.children[0].children[0].textContent = cardName;
        cardPreview.children[2].children[0].innerHTML = `[${cardType}]<br>${cardDescription}`;
        cardPreview.children[3].textContent = `ATT: ${attack} | DEF: ${defense}`;
    });
});
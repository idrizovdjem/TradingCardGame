const cards = Array.from(document.querySelectorAll('div.small-card'));
const cardPreview = document.querySelector('div.card-preview');
cards.map(card => {
    card.addEventListener('mouseover', (event) => {
        cardPreview.children[1].children[0].src = card.children[0].src;
        cardPreview.children[0].children[0].textContent = event.currentTarget.children[1].value;
        cardPreview.children[2].children[0].textContent = event.currentTarget.children[2].value;
        const attack = event.currentTarget.children[3].value;
        const defense = event.currentTarget.children[4].value;

        cardPreview.children[3].textContent = `ATT: ${attack} | DEF: ${defense}`;
    });
});
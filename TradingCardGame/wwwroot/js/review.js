const decisionLinks = Array.from(document.querySelectorAll('a'));
const channelName = sessionStorage.getItem('selectedChannel');
decisionLinks.map(link => {
    link.href += `&channelName=${channelName}`;
});
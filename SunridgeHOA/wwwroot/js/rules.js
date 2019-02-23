$('div.card-header button').click(() => {
    let current = $(this);
    let target = current.data('target');
    console.log(target);
    $(target).addClass('show');
});

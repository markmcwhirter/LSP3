function AddBook(formdata) {

    var data = JSON.stringify(Object.fromEntries(formdata));

    const book = JSON.parse(data);
    deleteFromObject('__RequestVerificationToken', book);

    let options = {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(book)
    };

    fetch(API_URL + "book/update", options)
        .then(
            response => { return 'Success'; }
        ).then(
            html => console.log(html)
        );
}
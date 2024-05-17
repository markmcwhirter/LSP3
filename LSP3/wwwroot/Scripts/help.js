function Help(data) {

    let options = {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: data
    };

    fetch(API_URL + "help", options)
        .then(
            response => { return response.statusText() }
        ).then(
            html => console.log(html)
        );
}
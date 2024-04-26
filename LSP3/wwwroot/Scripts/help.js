function Help(data) {

    let options = {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: data
    };

    fetch("http://164.92.99.186:8080/api/help", options)
        .then(
            response => { return response.statusText() }
        ).then(
            html => console.log(html)
        );
}
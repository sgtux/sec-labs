// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function removePost(id) {
    if (confirm('Remove this post ?'))
        window.location = '/Post/Delete/' + id;
}

function logout() {
    window.location = '/Account/Logout';
}

function searchPosts() {
    const elem = document.getElementById('txtSearch')
    window.location = '/Post?search=' + elem.value
}

eval("function checkReadiness() { window.location = '/CheckReadiness?ip=' + document.getElementById('ip').value }")

function sendComment(postId) {
    const elem = document.getElementById('comment-text-' + postId)
    const text = elem.value
    if (text) {
        fetch('/Comment', {
            method: 'POST',
            body: commentToXml(postId, text),
            headers: {
                'Content-type': 'application/xml'
            }
        }).then(() => {
            window.location.reload()
            elem.value = ''
        }).catch(err => console.log(err))
    }
}

function removeComment(id) {
    fetch('/Comment/' + id, { method: 'DELETE' })
        .then(() => window.location.reload())
        .catch(err => console.log(err))
}

function commentToXml(postId, text) {
    return `<?xml version="1.0"?><CreateCommentModel xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema"><PostId>${postId}</PostId><Text>${text}</Text></CreateCommentModel>`
}
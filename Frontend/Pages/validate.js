const token = localStorage.getItem("session");
if (!token){
 location.replace("login.html")
}

const usuario = JSON.parse(localStorage.getItem("usario"));

 if (Object.keys(usuario).length > 2){
    const header = document.querySelector(".header-content")
    const pUser = document.createElement('p');
    pUser.classList.add("username");
    pUser.innerHTML = usuario.username;
    header.prepend(pUser);

 }

function salir(){
    localStorage.removeItem("session");
    localStorage.removeItem("usario");
    location.replace("login.html")
}
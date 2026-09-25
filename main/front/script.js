function cargarSesion(){
  try{ return JSON.parse(localStorage.getItem('moroncito_sesion') || 'null'); }
  catch(e){ return null; }
}

const usuario = cargarSesion();
if(!usuario){
  window.location.href = 'login.html';
} else {

document.getElementById('sesion').innerHTML =
  `<span>${usuario.nombre} ${usuario.apellido} · ${usuario.barrio} · DNI ${usuario.dni}</span>
   <button type="button" id="btnSalir">Salir</button>`;
document.getElementById('btnSalir').addEventListener('click', () => {
  localStorage.removeItem('moroncito_sesion');
  window.location.href = 'login.html';
});
const nombreCampo = document.getElementById('nombre');
if(nombreCampo) nombreCampo.value = `${usuario.nombre} ${usuario.apellido}`;

const CATEGORIAS = [
  "Alumbrado","Calles y veredas","Limpieza y arbolado","Ruidos molestos",
  "Plazas y parques","Pluviales","Ordenamiento público","Fiscalización"
];

const catsEl = document.getElementById('categorias');
CATEGORIAS.forEach((cat, i) => {
  const id = 'cat' + i;
  const wrap = document.createElement('span');
  wrap.className = 'cat-pill';
  wrap.innerHTML = `<input type="radio" name="categoria" id="${id}" value="${cat}">
    <label for="${id}">${cat}</label>`;
  catsEl.appendChild(wrap);
});

function cargar(){
  try{
    return JSON.parse(localStorage.getItem('moroncito_incidencias') || '[]');
  }catch(e){ return []; }
}
function guardar(lista){
  try{ localStorage.setItem('moroncito_incidencias', JSON.stringify(lista)); }
  catch(e){ console.error('No se pudo guardar', e); }
}

function render(){
  const lista = cargar();
  const ul = document.getElementById('lista');
  ul.innerHTML = '';
  if(!lista.length){
    ul.innerHTML = '<p class="vacio">Todavía no hay reportes. El primero puede ser el tuyo.</p>';
    return;
  }
  lista.slice().reverse().forEach(rep => {
    const li = document.createElement('li');
    li.className = 'item';
    li.innerHTML = `
      <div class="item-top">
        <div>
          <span class="tag">${rep.categoria} · #${rep.id}</span>
          <h4>${rep.titulo}</h4>
        </div>
        <span class="estado">Pendiente</span>
      </div>
      ${rep.detalle ? `<p class="desc">${rep.detalle}</p>` : ''}
      <p class="meta">${rep.direccion} · ${rep.fecha}</p>
    `;
    ul.appendChild(li);
  });
}

document.getElementById('formIncidencia').addEventListener('submit', e => {
  e.preventDefault();
  const cat = document.querySelector('input[name=categoria]:checked');
  const titulo = document.getElementById('titulo').value.trim();
  const direccion = document.getElementById('direccion').value.trim();
  const detalle = document.getElementById('detalle').value.trim();

  document.getElementById('errCat').style.display = cat ? 'none' : 'block';
  document.getElementById('errTitulo').style.display = titulo ? 'none' : 'block';
  document.getElementById('errDireccion').style.display = direccion ? 'none' : 'block';
  if(!cat || !titulo || !direccion) return;

  const lista = cargar();
  const id = 1000 + lista.length;
  lista.push({
    id, categoria: cat.value, titulo, direccion, detalle,
    fecha: new Date().toLocaleDateString('es-AR', {day:'2-digit', month:'2-digit', year:'numeric'})
  });
  guardar(lista);

  const aviso = document.getElementById('aviso');
  aviso.textContent = `Reporte enviado. Número de seguimiento: #${id}.`;
  aviso.style.display = 'block';

  e.target.reset();
  render();
});

render();

}
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

// Ahora los reportes viven en la base de datos, no en localStorage.
async function cargarReportes(){
  try{
    const resp = await fetch('/api/reportes');
    if(!resp.ok) throw new Error('Respuesta no OK del servidor');
    return await resp.json();
  }catch(e){
    console.error('No se pudieron cargar los reportes', e);
    return null; // null = error de conexión, distinto de "lista vacía"
  }
}

async function render(){
  const ul = document.getElementById('lista');
  const lista = await cargarReportes();

  if(lista === null){
    ul.innerHTML = '<p class="vacio">No se pudo conectar con el servidor. Intentá recargar la página.</p>';
    return;
  }
  if(!lista.length){
    ul.innerHTML = '<p class="vacio">Todavía no hay reportes. El primero puede ser el tuyo.</p>';
    return;
  }

  ul.innerHTML = '';
  lista.forEach(rep => {
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

document.getElementById('formIncidencia').addEventListener('submit', async e => {
  e.preventDefault();
  const cat = document.querySelector('input[name=categoria]:checked');
  const titulo = document.getElementById('titulo').value.trim();
  const direccion = document.getElementById('direccion').value.trim();
  const detalle = document.getElementById('detalle').value.trim();

  document.getElementById('errCat').style.display = cat ? 'none' : 'block';
  document.getElementById('errTitulo').style.display = titulo ? 'none' : 'block';
  document.getElementById('errDireccion').style.display = direccion ? 'none' : 'block';
  if(!cat || !titulo || !direccion) return;

  const aviso = document.getElementById('aviso');
  aviso.style.display = 'none';

  try{
    const resp = await fetch('/api/reportes', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({
        categoria: cat.value,
        titulo,
        direccion,
        detalle,
        usuarioId: usuario.usuarioId
      })
    });

    if(!resp.ok){
      const err = await resp.json().catch(() => ({}));
      throw new Error(err.mensaje || 'No se pudo crear el reporte');
    }

    const creado = await resp.json();

    aviso.textContent = `Reporte enviado. Número de seguimiento: #${creado.id}.`;
    aviso.style.display = 'block';

    e.target.reset();
    render();
  }catch(err){
    console.error(err);
    aviso.textContent = 'No se pudo enviar el reporte. Probá de nuevo en un momento.';
    aviso.style.display = 'block';
  }
});

render();

}

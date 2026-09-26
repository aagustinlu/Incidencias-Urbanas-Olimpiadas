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

const CATEGORIAS = [
  "Alumbrado","Calles y veredas","Limpieza y arbolado","Ruidos molestos",
  "Plazas y parques","Pluviales","Ordenamiento público","Fiscalización"
];

const ICONOS = {
  "Alumbrado": "icon-bulb",
  "Calles y veredas": "icon-road",
  "Limpieza y arbolado": "icon-leaf",
  "Ruidos molestos": "icon-megaphone",
  "Plazas y parques": "icon-tree",
  "Pluviales": "icon-drop",
  "Ordenamiento público": "icon-shield",
  "Fiscalización": "icon-clipboard"
};

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
let TODOS = [];          // último listado que devolvió el servidor
let filtroCategoria = null;
let filtroTexto = '';

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

function renderFiltros(){
  const cont = document.getElementById('filtrosCategoria');
  if(!cont) return;
  const presentes = [...new Set(TODOS.map(r => r.categoria))];

  cont.innerHTML = '';
  if(!presentes.length) return;

  const chipTodas = document.createElement('button');
  chipTodas.type = 'button';
  chipTodas.className = 'chip-filtro' + (filtroCategoria === null ? ' activo' : '');
  chipTodas.textContent = 'Todas';
  chipTodas.addEventListener('click', () => { filtroCategoria = null; renderFiltros(); renderLista(); });
  cont.appendChild(chipTodas);

  presentes.forEach(cat => {
    const b = document.createElement('button');
    b.type = 'button';
    b.className = 'chip-filtro' + (filtroCategoria === cat ? ' activo' : '');
    b.textContent = cat;
    b.addEventListener('click', () => {
      filtroCategoria = (filtroCategoria === cat ? null : cat);
      renderFiltros();
      renderLista();
    });
    cont.appendChild(b);
  });
}

function renderResumen(){
  const el = document.getElementById('resumen');
  if(!el) return;
  const total = TODOS.length;
  if(!total){ el.textContent = ''; return; }

  const porCategoria = {};
  TODOS.forEach(r => { porCategoria[r.categoria] = (porCategoria[r.categoria] || 0) + 1; });
  const [masFrecuente] = Object.entries(porCategoria).sort((a, b) => b[1] - a[1]);

  el.textContent = total === 1
    ? '1 reporte activo'
    : `${total} reportes activos · lo más reportado: ${masFrecuente[0]}`;
}

function listaFiltrada(){
  return TODOS.filter(r => {
    const okCategoria = !filtroCategoria || r.categoria === filtroCategoria;
    const okTexto = !filtroTexto || `${r.titulo} ${r.direccion}`.toLowerCase().includes(filtroTexto);
    return okCategoria && okTexto;
  });
}

function renderLista(){
  const ul = document.getElementById('lista');
  const items = listaFiltrada();

  if(!items.length){
    ul.innerHTML = TODOS.length
      ? '<p class="vacio">No hay reportes que coincidan con la búsqueda.</p>'
      : '<p class="vacio">Todavía no hay reportes. El primero puede ser el tuyo.</p>';
    return;
  }

  ul.innerHTML = '';
  items.forEach(rep => {
    const li = document.createElement('li');
    li.className = 'item';
    const iconId = ICONOS[rep.categoria] || 'icon-clipboard';

    li.innerHTML = `
      <button type="button" class="item-top" aria-expanded="false">
        <span class="item-icono"><svg aria-hidden="true"><use href="#${iconId}"></use></svg></span>
        <span class="item-texto">
          <span class="tag">${rep.categoria} · #${rep.id}</span>
          <span class="item-titulo">${rep.titulo}</span>
          <span class="meta">${rep.direccion} · ${rep.fecha}</span>
        </span>
        <span class="estado">Pendiente</span>
        ${rep.detalle ? '<span class="item-chevron" aria-hidden="true">›</span>' : ''}
      </button>
      ${rep.detalle ? `<div class="item-detalle"><p>${rep.detalle}</p></div>` : ''}
    `;

    if(rep.detalle){
      const btn = li.querySelector('.item-top');
      btn.addEventListener('click', () => {
        const abierto = li.classList.toggle('abierto');
        btn.setAttribute('aria-expanded', String(abierto));
      });
    }

    ul.appendChild(li);
  });
}

async function render(){
  const data = await cargarReportes();
  const ul = document.getElementById('lista');
  const resumen = document.getElementById('resumen');

  if(data === null){
    ul.innerHTML = '<p class="vacio">No se pudo conectar con el servidor. Intentá recargar la página.</p>';
    if(resumen) resumen.textContent = '';
    const filtros = document.getElementById('filtrosCategoria');
    if(filtros) filtros.innerHTML = '';
    return;
  }

  TODOS = data;
  renderFiltros();
  renderResumen();
  renderLista();
}

const buscador = document.getElementById('buscador');
if(buscador){
  buscador.addEventListener('input', e => {
    filtroTexto = e.target.value.trim().toLowerCase();
    renderLista();
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
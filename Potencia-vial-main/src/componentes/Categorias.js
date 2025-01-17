import style from './css/Categorias.module.css'
import imgBulldozer from '../imagenes/categorias/bulldozer.png'
import imgCargadorFrontalOruga from '../imagenes/categorias/cargadoresFrontalesOruga.png'
import imgExcavadoraOruga from '../imagenes/categorias/excavadorasOrugas.png'
import imgCargadorFrontalNeu from '../imagenes/categorias/cargadoresFrontalesNeumaticos.png'
import imgRetroexcavadora from '../imagenes/categorias/retroexcavadoras.png'
import imgMotoniveladores from '../imagenes/categorias/motoniveladoras.png'
import imgCompactadores from '../imagenes/categorias/compactadores.png'
import imgRegadorasAsfalto from '../imagenes/categorias/terminadoraAsfalto.png'
import imgTerminadorasAsfalto from '../imagenes/categorias/terminadoraAsfalto.png'
import imgBarredoras from '../imagenes/categorias/barredoraVial.png'
import imgGravilladoras from '../imagenes/categorias/gravilladoras.png'
import imgTrituuradoras from '../imagenes/categorias/trituradoras.png'
import imgPlantasHormigon from '../imagenes/categorias/plantasHormigon.png'
import imgLavaderoAridos from '../imagenes/categorias/lavadoresAridos.png'
import imgCamionVolcadora from '../imagenes/categorias/camionVolcadora.png'
import imgCompresores from '../imagenes/categorias/compresor.png'
import imgGeneradores from '../imagenes/categorias/generador.png'
import imgMinicargadores from '../imagenes/categorias/minicargadores.png'
import imgGruas from '../imagenes/categorias/grua.png'
import imgAccesorios from '../imagenes/categorias/'
import imgAutoelevadores from '../imagenes/categorias/autoelevadores.png'



import { useSelector } from 'react-redux'




const Categorias = () => {


    const imagenes = useSelector(state=>state.imagenes.imagenes)

    return (
        <div className={style.contenedor}>
            <article className={style.article}>
                <a href='' className={style.a}>
                    <figure><img src={imgBulldozer}></img></figure>
                    <span>Bulldozer</span>
                </a>
            </article>
            <article className={style.article}>
                <a className={style.a} href=''>
                    <figure><img></img></figure>
                    <span>CARGADORES FRONTALES SOBRE ORUGAS</span>
                </a>
            </article>
            <article className={style.article}>
                <a href='' className={style.a}>
                    <figure><img src={imgExcavadoraOruga}></img></figure>
                    <span>EXCAVADORAS DE ORUGAS</span>
                </a>
            </article>
            <article className={style.article}>
                <a href='' className={style.a}>
                    <figure><img src={imgCargadorFrontalNeu}></img></figure>
                    <span>CARGADORES FRONTALES SOBRE NEUMÁTICOS</span>
                </a>
            </article>
            <article className={style.article}>
                <a href='' className={style.a}>
                    <figure><img src={imgRetroexcavadora}></img></figure>
                    <span>RETROEXCAVADORAS COMBINADAS</span>
                </a>
            </article>
            <article className={style.article}>
                <a href='' className={style.a}>
                    <figure><img src={imgMotoniveladores}></img></figure>
                    <span>MOTONIVELADORAS</span>
                </a>
            </article>
            <article className={style.article}>
                <a href='' className={style.a}>
                    <figure><img src={imgCompactadores}></img></figure>
                    <span>COMPACTADORES</span>
                </a>
            </article>
            <article className={style.article}>
                <a href='' className={style.a}>
                    <figure><img src={imgRegadorasAsfalto}></img></figure>
                    <span>REGADORES DE ASFALTO</span>
                </a>
            </article>
            <article className={style.article}>
                <a href='' className={style.a}>
                    <figure><img src={imgTerminadorasAsfalto}></img></figure>
                    <span>TERMINADORAS DE ASFALTO</span>
                </a>
            </article>
            <article className={style.article}>
                <a href='' className={style.a}>
                    <figure><img src={imgBarredoras}></img></figure>
                    <span>BARREDORAS VIALES</span>
                </a>
            </article>
            <article className={style.article}>
                <a href='' className={style.a}>
                    <figure><img src={imgGravilladoras}></img></figure>
                    <span>GRAVILLADORAS </span>
                </a>
            </article>
            
            <article className={style.article}>
                <a href='' className={style.a}>
                    <figure><img src={imgTrituuradoras}></img></figure>
                    <span>TRITURADORAS</span>
                </a>
            </article>
            <article className={style.article}>
                <a href='' className={style.a}>
                    <figure><img src={imgPlantasHormigon}></img></figure>
                    <span>PLANTAS DE HORMIGÓN/ Asfalto</span>
                </a>
            </article>
            <article className={style.article}>
                <a href='' className={style.a}>
                    <figure><img src={imgLavaderoAridos}></img></figure>
                    <span>LAVADEROS DE ÁRIDOS</span>
                </a>
            </article>
            <article className={style.article}>
                <a className={style.a} href=''>
                    <figure><img src={imgCamionVolcadora}></img></figure>
                    <span>CAMIONES CON VOLCADORA</span>
                </a>
            </article>
            <article className={style.article}>
                <a className={style.a} href=''>
                    <figure><img src={imgCompresores}></img></figure>
                    <span>COMPRESORES</span>
                </a>
            </article>
            <article className={style.article}>
                <a className={style.a} href=''>
                    <figure><img src={imgGeneradores}></img></figure>
                    <span>GENERADORES</span>
                </a>
            </article>
            <article className={style.article}>
                <a className={style.a} href=''>
                    <figure><img src={imgGruas}></img></figure>
                    <span>GRÚAS</span>
                </a>
            </article>
            <article className={style.article}>
                <a className={style.a} href=''>
                    <figure><img src={imgAutoelevadores}></img></figure>
                    <span>AUTOELEVADORES</span>
                </a>
            </article>
            <article className={style.article}>
                <a className={style.a} href=''>
                    <figure><img src={imgMinicargadores}></img></figure>
                    <span>MINICARGADORES</span>
                </a>
            </article>
            <article className={style.article}>
                <a className={style.a} href=''>
                    <figure><img src={imgCargadorFrontalNeu}></img></figure>
                    <span>ACCESORIOS </span>
                </a>
            </article>
        </div>

    )
}

export default Categorias
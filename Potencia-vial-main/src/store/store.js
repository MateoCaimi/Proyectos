import { configureStore } from "@reduxjs/toolkit";
import maquinasReducer  from "../features/maquinasSlice";
import tipoReducer from '../features/tiposSlice';
import imagenesReducer from '../features/categoriasSlice'

export const store = configureStore({
   reducer:{
      maquinas: maquinasReducer,
      tipos: tipoReducer,
      imagenes: imagenesReducer

   }
})
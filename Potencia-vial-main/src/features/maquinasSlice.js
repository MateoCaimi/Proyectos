import { createSlice } from "@reduxjs/toolkit";

const initialState = {
    maquinas: [],
    maquinasXTipo:[]
}

export const maquinaSlice = createSlice({
    name: "Maquina",
    initialState,
    reducers: {
        guardarMaquinas: (state, action) => {
            state.maquinas = action.payload
        },
        guardarMaquinasXTipo:(state, action)=>{
            state.maquinasXTipo = action.payload
        },
        agregarMaquina: (state, action) => {
            state.maquinas.push(action.payload)
        },
        eliminarMaquina: (state, action) => {
            state.maquinas = state.maquinas.filter(m => m.nombre !== action.payload)
        }
        
      
    }
})

export const { agregarMaquina, guardarMaquinas, guardarMaquinasXTipo, eliminarMaquina } = maquinaSlice.actions;

export default maquinaSlice.reducer;
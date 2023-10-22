var express = require("express");
const asyncHandler = require("express-async-handler");
const dataBase = require("../BD/mysql");
let controller = require("./tipoController");
const { database } = require("../config/config");

exports.maquinas_getAll = asyncHandler(async (req, res, next) => {
  const result = await dataBase.getMaquinas();
  if (!result) {
    throw new Error("Error en el resultado");
  } else {
    res.status(200).json(result);
  }
});

exports.maquina_get = asyncHandler(async (req, res, next) => {
  console.log("Entro al controller");
  const result = await dataBase.getMaquina(req.params.id);
  console.log(result);
  if (!result) {
    throw new Error("Error en el resultado");
  } else {
    res.status(200).json(result);
  }
});

exports.maquinasAndType = asyncHandler(async (req, res, next) => {
  // Obtener todos los tipos, máquinas, multimedias de máquinas y multimedias de manera eficiente
  const [tipos, maquinas, multimediasMaquinas, multimedias] = await Promise.all([
    dataBase.getTipos(),
    dataBase.getMaquinasActivas(),
    dataBase.getMultimediaByMaquina(),
    dataBase.getMultimedia(),
  ]);

  // Crear un mapa para relacionar multimedia con máquinas
  const multimediaMap = new Map();
  for (const multimediaMaquina of multimediasMaquinas) {
    if (!multimediaMap.has(multimediaMaquina.MaquinaId)) {
      multimediaMap.set(multimediaMaquina.MaquinaId, []);
    }
    multimediaMap.get(multimediaMaquina.MaquinaId).push(multimediaMaquina.MultimediaId);
  }

  // Crear la estructura de datos resultante
  const result = {
    Tipos: tipos.map(tipo => ({
      Id: tipo.Id,
      Nombre: tipo.Nombre,
      Descripcion: tipo.Descripcion,
      Maquinas: maquinas
        .filter(maquina => maquina.TipoId === tipo.Id)
        .map(maquina => ({
          Id: maquina.Id,
          Modelo: maquina.Modelo,
          TipoId: maquina.TipoId,
          Condicion: maquina.Condicion,
          Altura: maquina.Altura,
          Largo: maquina.Largo,
          Ancho: maquina.Ancho,
          CargaMaxima: maquina.CargaMaxima,
          MaquinaMultimedia: (multimediaMap.get(maquina.Id) || []).map(multimediaId => {
            const multimedia = multimedias.find(item => item.Id === multimediaId);
            return multimedia; // Agregar el elemento multimedia correspondiente a la lista
          }),
        })),
    })),
  };

  if (!result) {
    throw new Error("Error en el resultado");
  } else {
    res.status(200).json({ result });
  }
});



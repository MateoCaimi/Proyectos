var express = require("express");
const asyncHandler = require("express-async-handler");
const dataBase = require("../BD/mysql");
const { database } = require("../config/config");

exports.eliminarMaquina = asyncHandler(async (req, res, next) => {
  const maquinaId = req.body.Id;

  const deleteMaquina = await dataBase.ejecutarConsulta(
    `DELETE FROM potencia_vial.Maquina where Id = ?`,
    [maquinaId]
  );
  const deleteMultimediaMaquina = await dataBase.ejecutarConsulta(
    `DELETE FROM potencia_vial.MaquinaMultimedia where MaquinaId = ?`,
    [maquinaId]
  );
  const result = {
    respone: {
      Code: 0,
      Message: "",
    },
  };
  if (!deleteMaquina && !deleteMultimediaMaquina) {
    throw new Error("Error en el resultado");
  } else {
    result.respone.Code = 200;
    result.respone.Message = `Maquina ${maquinaId} eliminada con exito`;
    res.status(200).json(result);
  }
});

exports.addMaquina = asyncHandler(async (req, res, next) => {
  const Modelo = req.body.Modelo;
  const TipoId = req.body.TipoId;
  const Condicion = req.body.Condicion;
  const Altura = req.body.Altura;
  const Largo = req.body.Largo;
  const Ancho = req.body.Ancho;
  const CargaMaxima = req.body.CargaMaxima;
  const descripcion = req.body.descripcion;
  const anio = req.body.anio;
  const Marca = req.body.marca;
  const Precio = req.body.Precio;

  const data = {
    Modelo,
    TipoId,
    Condicion,
    Altura,
    Largo,
    Ancho,
    CargaMaxima,
    descripcion,
    anio,
    Marca,
    Precio,
  };
  const result = await dataBase.addMaquina(data);
  if (!result) {
    throw new Error("Error en el resultado");
  } else {
    res.status(200).json(result);
  }
});

exports.maquinas_getAll = asyncHandler(async (req, res, next) => {
  const result = await dataBase.getMaquinas();
  if (!result) {
    throw new Error("Error en el resultado");
  } else {
    res.status(200).json(result);
  }
});

exports.maquina_get = asyncHandler(async (req, res, next) => {
  try {
    // await dataBase.conectar();
    const maquina = await dataBase.ejecutarConsulta(
      `SELECT * FROM potencia_vial.Maquina where Id = ?`,
      [req.params.id]
    );
    const multimediasMaquinas = await dataBase.ejecutarConsulta(
      `SELECT * FROM potencia_vial.MaquinaMultimedia where MaquinaId = ?`,
      [maquina[0].Id]
    );
    const multimedias = await dataBase.ejecutarConsulta(
      `SELECT * from potencia_vial.Multimedia`
    );
    //await dataBase.desconectar();

    const multimediaMap = new Map();
    for (const multimediaMaquina of multimediasMaquinas) {
      if (!multimediaMap.has(multimediaMaquina.MaquinaId)) {
        multimediaMap.set(multimediaMaquina.MaquinaId, []);
      }
      multimediaMap
        .get(multimediaMaquina.MaquinaId)
        .push(multimediaMaquina.MultimediaId);
    }

    const result = {
      Id: maquina[0].Id,
      Modelo: maquina[0].Modelo,
      TipoId: maquina[0].TipoId,
      Condicion: maquina[0].Condicion,
      Altura: maquina[0].Altura,
      Largo: maquina[0].Largo,
      Ancho: maquina[0].Ancho,
      CargaMaxima: maquina[0].CargaMaxima,
      Anio: maquina[0].anio,
      Descripcion: maquina[0].descripcion,
      Marca: maquina[0].marca,
      Precio: maquina[0].Precio,
      MaquinaMultimedia: (multimediaMap.get(maquina[0].Id) || []).map(
        (multimediaId) => {
          const multimedia = multimedias.find(
            (item) => item.Id === multimediaId
          );
          return {
            Id: multimedia.Id,
            Nombre: multimedia.Nombre,
            Tipo: multimedia.Tipo,
          }; // Agregar el elemento multimedia correspondiente a la lista
        }
      ),
    };
    if (!result) {
      throw new Error("No se encontró la máquina.");
    } else {
      res.status(200).json(result);
    }
  } catch (error) {
    next(error);
  }
});

exports.maquinasAndType = asyncHandler(async (req, res, next) => {
  // Obtener todos los tipos, máquinas, multimedias de máquinas y multimedias de manera eficiente
  const [tipos, maquinas, multimediasMaquinas, multimedias] = await Promise.all(
    [
      dataBase.ejecutarConsulta(`SELECT * from potencia_vial.Tipo`),
      dataBase.ejecutarConsulta(`SELECT * from potencia_vial.Maquina`),
      dataBase.ejecutarConsulta(
        `SELECT * from potencia_vial.MaquinaMultimedia`
      ),
      dataBase.ejecutarConsulta(`SELECT * from potencia_vial.Multimedia`),
    ]
  );

  // Crear un mapa para relacionar multimedia con máquinas
  const multimediaMap = new Map();
  for (const multimediaMaquina of multimediasMaquinas) {
    if (!multimediaMap.has(multimediaMaquina.MaquinaId)) {
      multimediaMap.set(multimediaMaquina.MaquinaId, []);
    }
    multimediaMap
      .get(multimediaMaquina.MaquinaId)
      .push(multimediaMaquina.MultimediaId);
  }

  // Crear la estructura de datos resultante
  const result = {
    Tipos: tipos.map((tipo) => ({
      Id: tipo.Id,
      Nombre: tipo.Nombre,
      Descripcion: tipo.Descripcion,
      Maquinas: maquinas
        .filter((maquina) => maquina.TipoId === tipo.Id)
        .map((maquina) => ({
          Id: maquina.Id,
          Modelo: maquina.Modelo,
          TipoId: maquina.TipoId,
          Condicion: maquina.Condicion,
          Altura: maquina.Altura,
          Largo: maquina.Largo,
          Ancho: maquina.Ancho,
          CargaMaxima: maquina.CargaMaxima,
          Anio: maquina.anio,
          Descripcion: maquina.descripcion,
          Marca: maquina.marca,
          Precio: maquina.Precio,
          MaquinaMultimedia: (multimediaMap.get(maquina.Id) || []).map(
            (multimediaId) => {
              const multimedia = multimedias.find(
                (item) => item.Id === multimediaId
              );
              return {
                Id: multimedia.Id,
                Nombre: multimedia.Nombre,
                Tipo: multimedia.Tipo,
              }; // Agregar el elemento multimedia correspondiente a la lista
            }
          ),
        })),
    })),
  };

  if (!result) {
    throw new Error("Error en el resultado");
  } else {
    res.status(200).json({ result });
  }
});

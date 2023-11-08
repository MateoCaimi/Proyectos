var express = require("express");
const asyncHandler = require("express-async-handler");
const dataBase = require("../BD/mysql");

exports.tipos_get = asyncHandler(async (req, res, next) => {
  const result = await dataBase.getTipos();
  if (!result) {
    throw new Error("Error en el resultado");
  } else {
    res.status(200).json(result);
  }
});

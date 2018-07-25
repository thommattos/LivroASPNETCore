﻿using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace SolucaoCapitulo01.Migrations
{
    public partial class FotoAcademico : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "Foto",
                table: "Academicos",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FotoMimeType",
                table: "Academicos",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Foto",
                table: "Academicos");

            migrationBuilder.DropColumn(
                name: "FotoMimeType",
                table: "Academicos");
        }
    }
}

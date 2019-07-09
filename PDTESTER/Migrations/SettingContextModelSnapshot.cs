﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using PDTESTER;
using System;

namespace PDTESTER.Migrations
{
    [DbContext(typeof(SettingContext))]
    partial class SettingContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125");

            modelBuilder.Entity("PDTESTER.JigModel", b =>
                {
                    b.Property<int>("JigModelID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Channel");

                    b.Property<float>("FailRate");

                    b.Property<bool>("IsJigEnable");

                    b.Property<bool>("IsSetInJig");

                    b.Property<string>("JigDescription");

                    b.Property<int>("JigID");

                    b.Property<int>("JigPos");

                    b.Property<int>("NGCounter");

                    b.Property<int>("OKCounter");

                    b.Property<int>("TestResult");

                    b.HasKey("JigModelID");

                    b.ToTable("JigModels");
                });

            modelBuilder.Entity("PDTESTER.SWSetting", b =>
                {
                    b.Property<uint>("SWSettingID")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("LOADTest");

                    b.Property<bool>("PDCTest");

                    b.Property<bool>("SBUTest");

                    b.Property<bool>("UD3Test");

                    b.Property<bool>("UO2Test");

                    b.Property<bool>("UO3Test");

                    b.Property<bool>("VCONNTest");

                    b.HasKey("SWSettingID");

                    b.ToTable("SWSettings");
                });

            modelBuilder.Entity("PDTESTER.ValueSetting", b =>
                {
                    b.Property<uint>("ValueSettingID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Key");

                    b.Property<string>("Value");

                    b.HasKey("ValueSettingID");

                    b.ToTable("ValueSettings");
                });
#pragma warning restore 612, 618
        }
    }
}

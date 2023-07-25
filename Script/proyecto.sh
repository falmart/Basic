#!/bin/bash
while true;
do
    clear
    echo -e "\e[32m Options (Select a number) \e[0m"
    echo 
    echo " 1) run"
    echo " 2) report"
    echo " 3) slides"
    echo " 4) show_report"
    echo " 5) show_slides"
    echo " 6) clean"
    echo " 7) quit" 
run() 
{
    echo -e "\e[32m Launching Moogle \e[0m"
    cd ..
    dotnet watch run --project MoogleServer
    echo "Press Enter to continue" 
    read
}

compile_report() 
{
    echo -e "\e[32m Compiling report \e[0m"
    cd .. 
    cd Informe
    latexmk -pdf Informe.tex
    echo "Press Enter to continue"
    read
}

compile_slides() 
{
    echo -e "\e[32m Compiling slide \e[0m"
    cd ..
    cd Presentacion
    latexmk -pdf Presentacion.tex
    echo "Press Enter to continue"
    read
}

show_report() 
{
    echo -e "\e[32m Showing report \e[0m"
    cd ..
    cd Informe
    xdg-open Informe.pdf
    echo "Press Enter to continue"
    read
}

show_slides() 
{
    echo -e "\e[32m Showing slides \e[0m"
    cd ..
    cd Presentacion
    xdg-open Presentacion.pdf
    echo "Press Enter to continue"
    read
}

clean() 
{
    echo -e "\e[32m Cleaning temporal files \e[0m"
    cd ..
    cd Informe
    rm informe.aux informe.fdb_latexmk informe.fls informe.log 
    cd ..
    cd Presentacion
    rm Presentacion.aux Presentacion.fdb_latexmk Presentacion.fls Presentacion.log Presentacion.nav Presentacion.snm Presentacion.toc Presentacion.out
    echo "Press Enter to continue"
    read
}

read option
    case $option in
        "1")
            run
            ;;
        "2")
            compile_report
            ;;
        "3")
            compile_slides
            ;;
        "4")
            show_report "report.pdf" 
            ;;
        "5")
            show_slides "slides.pdf" 
            ;;
        "6")
            clean
            ;;
        "7")
            echo -e "\e[32m Closing \e[0m"
            break
            ;;
    esac
done
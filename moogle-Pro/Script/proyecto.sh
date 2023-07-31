#! /bin/bash

    function run {
     cd ..
     make dev
    }

    function report {
     cd .. 
     cd Informe
     pdflatex Informe.tex
    }

    function slides {
     cd ..
     cd Presentación
     pdflatex Presentación.tex
    }

    function show_report {
     cd ..
     cd Informe
     if ! [ -f ./Informe.pdf ];
     then
        pdflatex Informe.tex
     fi
     evince Informe.pdf
    }

    function show_slides {
     cd ..
     cd Presentación
     if ![ -f ./Presentación.pdf ];
     then
        pdflatex Presentación.tex
     fi
     evince Presentación.pdf
    }

    function clean {
     cd ..
     cd MoogleEngine
     if [ -d ./bin -a -d ./obj ]
     then
        rm -r ./bin ./obj
     fi
     cd ..
     cd MoogleServer
     if [ -d ./bin -a -d ./obj ]
     then
        rm -r ./bin ./obj
     fi
     find ../ -depth -name "*.aux" -type f -delete
     find ../ -depth -name "*.log" -type f -delete
     find ../ -depth -name "*.out" -type f -delete
     find ../ -depth -name "*.toc" -type f -delete
     find ../ -depth -name "*.synctex.gz" -type f -delete
     find ../ -depth -name "*.fls" -type f -delete
     find ../ -depth -name "*.fdb_latexmk" -type f -delete
     find ../ -depth -name "*.nav" -type f -delete
     find ../ -depth -name "*.snm" -type f -delete
     find ../ -depth -name "*.vrb" -type f -delete
     echo "Los archivos auxiliares han sido eliminados"
    }

    case "$options" in
         "run")
             run
             ;;
         "report")
             report
             ;;
         "slides")
             slides
             ;;   
         "show_report")
             show_report
             ;;
         "show_slides")
             show_slides
             ;;
         "clean")
             clean
             ;;    
         *)
         echo "La opción elegida es inválida, elija una de las siguientes: run, report, slides, show_report, show_slides, clean"
             exit 1
esac

exit 0


    
/*
This file in the main entry point for defining grunt tasks and using grunt plugins.
Click here to learn more. https://go.microsoft.com/fwlink/?LinkID=513275&clcid=0x409
*/
module.exports = function (grunt) {
    grunt.initConfig({
        ts: {
            app: {
                tsconfig: "./src/tsconfig.json",
                passTrough: true,
                options: {
                    additionalFlags: '--skipLibCheck'
                }
            }
        },
        less: {
            style: {
                options: {
                    sourceMap: true,
                    sourceMapFileInline: true,
                    compress: false,
                    yuicompress: false,
                    optimization: 2
                },
                files: {
                    'src/css/style.css': [
                        'src/**/*.less' //include all less files (directives etc.)
                    ]
                }
            }
        },
        watch: {
            grunt: {
                files: ['gruntfile.js'],
                options: {
                    nospawn: true,
                    livereload: true
                }
            },
            modules: {
                files: ['src/**/*.ts'],
                tasks: ['ts', 'merge-json', 'concat', 'copy:dist'],
                options: {
                    nospawn: true,
                    livereload: false
                }
            },
            less: {
                files: ['src/**/*.less'],
                tasks: ['less', 'cssmin:minify', 'copy:dist'],
                options: {
                    nospawn: true,
                    livereload: false
                }
            },
            html: {
                files: ['src/**/*.html'],
                tasks: ['copy:dist'],
                options: {
                    nospawn: true,
                    livereload: false
                }
            },
            translations: {
                files: ['src/**/*.translation.json'],
                tasks: ['merge-json', 'copy:dist'],
                options: {
                    nospawn: true,
                    livereload: false
                }
            }
        },

        "merge-json": {
            i18n: {
                files: (function () {
                    var map = {};
                    var languages = ['en_US', 'de_DE'];
                    languages.forEach(function (language) {
                        var src = [
                            // include all *.translation.json files (which might be in a subfolder on a directive or sth)
                            'src/**/*' + language + '.translation.json',
                        ];
                        var dest = 'src/assets/translations/' + language + '.json';
                        map[dest] = src;
                    });
                    return map;
                } ())
            }
        },
        copy: {
            external: {
                files: [
                    {
                        expand: true,
                        flatten: true,
                        src: [
                            './node_modules/leaflet/dist/images/**',
                        ],
                        cwd: './',
                        dest: './src/css/images/',
                        filter: 'isFile'
                    },
                ],
            },
            dist: {
                files: [
                    {
                        expand: true,
                        flatten: false,
                        src: [
                            './**/*.html',
                            './**/*.css',
                            './**/*.js',
                            './**/img/**',
                            './**/assets/**',
                            '!./**/tsconfig.json',
                            './**/css/images/**',
                            './translations/**'
                        ],
                        cwd: 'src/',
                        dest: 'dist/',
                        filter: 'isFile'
                    },
                    {
                        src: 'src/index.live.html',
                        dest: 'dist/index.html'
                    },
                    {
                        expand: true,
                        flatten: false,
                        src: [
                            'bower_components/axoom-web-framework/**'
                        ],
                        dest: 'dist/'
                    },

                ],
            },
        },
        rename: {
            main: {
              files: [
                    {
                        src: 'dist/bower_components/axoom-web-framework/dist/locales/angular-locale_de-DE.js',
                        dest: 'dist/bower_components/axoom-web-framework/dist/locales/angular-locale_de-DE.js'
                    },
                    {
                        src: 'dist/bower_components/axoom-web-framework/dist/locales/angular-locale_en-US.js',
                        dest: 'dist/bower_components/axoom-web-framework/dist/locales/angular-locale_en-US.js'
                    },
                ]
            }
          },
        uglify: {
            options: {
                compress: false,
                mangle: false
            },
            apps: {
                files: [
                    {
                        expand: true,
                        cwd: 'src/',
                        src: ['**/app.js'],
                        dest: 'src/',
                        ext: '.min.js',
                    },
                ]
            },
        },
        cssmin: {
            minify: {
                files: {
                    'src/css/style.min.css': ['src/css/style.css']
                }
            }
        },
        clean: {
            options: {
                force: true
            },
            dist: [
                'wwwroot/**',
            ]
        },
        concat: {
            libs: {
                src: [
                    'node_modules/leaflet/dist/leaflet.js', // Include OpenLayers for Map
                    'node_modules/leaflet.markercluster/dist/leaflet.markercluster.js', // Include OpenLayers for Map
                    'bower_components/Leaflet.EasyButton/src/easy-button.js', // Include OpenLayers for Map
                    'bower_components/moment/moment.js',
                    'bower_components/moment/locale/de.js',
                    'bower_components/angular-moment/angular-moment.js',
                    'bower_components/Snap.svg/dist/snap.svg-min.js',
                    'bower_components/ngInfiniteScroll/build/ng-infinite-scroll.js',
                    'bower_components/angular-confirm-modal/angular-confirm.min.js',
                    'bower_components/ladda/dist/spin.min.js',
                    'bower_components/ladda/dist/ladda.min.js',
                    'bower_components/angular-ladda/dist/angular-ladda.min.js',
                    'bower_components/ng-tags-input/ng-tags-input.min.js',
                    'bower_components/angular-ui-sortable/sortable.min.js',
                    'bower_components/ng-file-upload-shim/ng-file-upload-shim.js',
                    'bower_components/ng-file-upload/ng-file-upload.js',
                    'src/assets/ng-fileupload/ng-fileupload.config.js',
                    'bower_components/ng-file-upload-shim/ng-file-upload-shim.js',
                    //'bower_components/ngSticky/dist/sticky.min.js',
                    'src/libs/checklist-model.js',
                    'bower_components/signalr/jquery.signalR.js',
                    'bower_components/angular-signalr-hub/signalr-hub.js',
                    'src/js/app.js',
                ],
                dest: 'src/js/app.js',
                nonull: true,
            }
        }
    });

    grunt.loadNpmTasks('grunt-ts');
    grunt.loadNpmTasks('grunt-contrib-less');
    grunt.loadNpmTasks('grunt-contrib-concat');
    grunt.loadNpmTasks('grunt-contrib-copy');
    grunt.loadNpmTasks('grunt-contrib-watch');
    grunt.loadNpmTasks('grunt-contrib-uglify');
    grunt.loadNpmTasks('grunt-contrib-cssmin');
    grunt.loadNpmTasks('grunt-bower-task');
    grunt.loadNpmTasks('grunt-merge-json');
    grunt.loadNpmTasks('grunt-typedoc');
    grunt.loadNpmTasks('grunt-contrib-clean');
    grunt.loadNpmTasks('grunt-contrib-rename');

    grunt.registerTask('default', ['less', 'cssmin', 'ts', 'concat', 'merge-json', 'copy:external', 'copy:dist']);
    grunt.registerTask('publish', ['clean:dist', 'default', 'uglify:apps', 'copy:dist', 'rename']);
};
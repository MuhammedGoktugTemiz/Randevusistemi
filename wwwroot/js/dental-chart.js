// Realistic Dental Chart - Horizontal Layout with Realistic Teeth
const upperTeeth = [18, 17, 16, 15, 14, 13, 12, 11, 21, 22, 23, 24, 25, 26, 27, 28];
const lowerTeeth = [48, 47, 46, 45, 44, 43, 42, 41, 31, 32, 33, 34, 35, 36, 37, 38];
let selectedTeeth = [];

function initDentalChart() {
    const chartContainer = document.getElementById('dentalChart');
    if (!chartContainer) return;

    chartContainer.innerHTML = createChartSvg();

    chartContainer.querySelectorAll('.tooth-shape').forEach(shape => {
        shape.addEventListener('click', () => toggleTooth(shape.dataset.tooth));
    });

    const toothNumbersInput = document.getElementById('toothNumbers');
    if (toothNumbersInput && toothNumbersInput.value) {
        selectedTeeth = toothNumbersInput.value.split(',').map(t => t.trim()).filter(Boolean);
        updateToothDisplay();
    }
}

function createChartSvg() {
    return `
        <svg viewBox="0 0 900 500" preserveAspectRatio="xMidYMid meet" class="dental-svg">
            <defs>
                <linearGradient id="toothGrad" x1="0%" y1="0%" x2="0%" y2="100%">
                    <stop offset="0%" stop-color="#ffffff"/>
                    <stop offset="50%" stop-color="#f8fafc"/>
                    <stop offset="100%" stop-color="#e9ecef"/>
                </linearGradient>
                <radialGradient id="mouthGrad" cx="50%" cy="50%">
                    <stop offset="0%" stop-color="#fafbfc"/>
                    <stop offset="100%" stop-color="#ffffff"/>
                </radialGradient>
            </defs>
            <rect width="900" height="500" fill="url(#mouthGrad)" rx="12"/>
            ${renderUpperJaw()}
            ${renderLowerJaw()}
        </svg>
    `;
}

function renderUpperJaw() {
    const startX = 100;
    const y = 150;
    const spacing = 45;
    const totalWidth = (upperTeeth.length - 1) * spacing;
    const centerX = 450;
    const startXCentered = centerX - totalWidth / 2;
    
    let result = '';
    
    // Üst diş eti çizgisi
    result += `<line x1="${startXCentered - 30}" y1="${y}" x2="${startXCentered + totalWidth + 30}" y2="${y}" 
                     stroke="rgba(236, 72, 153, 0.2)" stroke-width="35" stroke-linecap="round" class="gum-line upper"/>`;
    
    upperTeeth.forEach((tooth, index) => {
        const x = startXCentered + (index * spacing);
        const type = getToothType(tooth);
        const size = getToothSize(type);
        
        result += createRealisticTooth(tooth, x, y, type, size, 45, true);
    });
    
    return result;
}

function renderLowerJaw() {
    const startX = 100;
    const y = 350;
    const spacing = 45;
    const totalWidth = (lowerTeeth.length - 1) * spacing;
    const centerX = 450;
    const startXCentered = centerX - totalWidth / 2;
    
    let result = '';
    
    // Alt diş eti çizgisi
    result += `<line x1="${startXCentered - 30}" y1="${y}" x2="${startXCentered + totalWidth + 30}" y2="${y}" 
                     stroke="rgba(14, 165, 233, 0.2)" stroke-width="35" stroke-linecap="round" class="gum-line lower"/>`;
    
    lowerTeeth.forEach((tooth, index) => {
        const x = startXCentered + (index * spacing);
        const type = getToothType(tooth);
        const size = getToothSize(type);
        
        result += createRealisticTooth(tooth, x, y, type, size, -35, false);
    });
    
    return result;
}

function createRealisticTooth(tooth, x, y, type, size, labelY, isUpper) {
    const toothPath = getRealisticToothShape(type, size, isUpper);
    const rotation = isUpper ? 0 : 180;
    
    return `
        <g class="tooth-shape" data-tooth="${tooth}" transform="translate(${x}, ${y})">
            <path d="${toothPath}" 
                  transform="rotate(${rotation})" 
                  fill="url(#toothGrad)" 
                  stroke="#b8c5d1" 
                  stroke-width="2.2"
                  class="tooth-path"/>
            <text x="0" y="${labelY}" 
                  text-anchor="middle" 
                  font-size="14" 
                  font-weight="700" 
                  fill="#475569"
                  class="tooth-label">${tooth}</text>
        </g>
    `;
}

function getToothType(number) {
    const code = number % 10;
    if (code === 1 || code === 2) return 'incisor';      // Kesici diş
    if (code === 3) return 'canine';                      // Köpek dişi
    if (code === 4 || code === 5) return 'premolar';      // Küçük azı
    return 'molar';                                        // Azı dişi
}

function getToothSize(type) {
    // Gerçekçi diş boyutları
    const sizes = {
        incisor: { width: 14, height: 36 },      // İnce ve uzun
        canine: { width: 15, height: 38 },       // Biraz daha geniş ve uzun
        premolar: { width: 18, height: 32 },     // Orta genişlik
        molar: { width: 22, height: 32 }         // En geniş
    };
    return sizes[type] || sizes.premolar;
}

function getRealisticToothShape(type, size, isUpper) {
    const { width, height } = size;
    const topCurve = height * 0.35;
    const bottomWidth = width * 0.88;
    const sideCurve = width * 0.15;
    
    const shapes = {
        // Kesici diş: ince, uzun, düz kenarlı, hafif yuvarlatılmış
        incisor: `M -${width} 0 
                  Q -${width * 0.5} -${topCurve} 0 -${topCurve * 1.1}
                  Q ${width * 0.5} -${topCurve} ${width} 0
                  L ${bottomWidth} ${height * 0.3}
                  Q ${bottomWidth} ${height * 0.7} ${bottomWidth * 0.7} ${height}
                  Q 0 ${height + 3} -${bottomWidth * 0.7} ${height}
                  Q -${bottomWidth} ${height * 0.7} -${bottomWidth} ${height * 0.3}
                  Z`,
        
        // Köpek dişi: sivri uçlu, biraz daha geniş, keskin kenarlar
        canine: `M -${width} 0 
                 Q -${width * 0.3} -${topCurve + 3} 0 -${topCurve * 1.3}
                 Q ${width * 0.3} -${topCurve + 3} ${width} 0
                 Q ${width * 0.7} ${height * 0.3} ${width * 0.5} ${height * 0.6}
                 Q 0 ${height + 4} -${width * 0.5} ${height * 0.6}
                 Q -${width * 0.7} ${height * 0.3} -${width} 0
                 Z`,
        
        // Küçük azı: geniş, kısa, yuvarlak, çiğneme yüzeyi düz
        premolar: `M -${width} 0 
                   Q -${width * 0.4} -${topCurve} 0 -${topCurve * 0.9}
                   Q ${width * 0.4} -${topCurve} ${width} 0
                   L ${bottomWidth} ${height * 0.25}
                   Q ${bottomWidth} ${height * 0.6} ${bottomWidth * 0.6} ${height}
                   Q 0 ${height + 2} -${bottomWidth * 0.6} ${height}
                   Q -${bottomWidth} ${height * 0.6} -${bottomWidth} ${height * 0.25}
                   Z`,
        
        // Azı dişi: en geniş, çoklu tüberkül, çiğneme yüzeyi girintili
        molar: `M -${width} 0 
                Q -${width * 0.6} -${topCurve} -${width * 0.2} -${topCurve * 0.8}
                Q ${width * 0.2} -${topCurve * 0.8} ${width * 0.6} -${topCurve}
                Q ${width} 0 ${width} 0
                L ${bottomWidth} ${height * 0.2}
                Q ${bottomWidth} ${height * 0.5} ${bottomWidth * 0.5} ${height}
                Q 0 ${height + 2} -${bottomWidth * 0.5} ${height}
                Q -${bottomWidth} ${height * 0.5} -${bottomWidth} ${height * 0.2}
                Z`
    };
    
    return shapes[type] || shapes.premolar;
}

function toggleTooth(toothNumber) {
    const index = selectedTeeth.indexOf(toothNumber);
    if (index > -1) {
        selectedTeeth.splice(index, 1);
    } else {
        selectedTeeth.push(toothNumber);
    }
    updateToothDisplay();
    updateToothInput();
    updateProcedureSelect();
}

function updateToothDisplay() {
    document.querySelectorAll('.tooth-shape').forEach(node => {
        const tooth = node.dataset.tooth;
        node.classList.toggle('active', selectedTeeth.includes(tooth));
    });
}

function updateToothInput() {
    const input = document.getElementById('toothNumbers');
    if (input) {
        input.value = selectedTeeth
            .sort((a, b) => parseInt(a, 10) - parseInt(b, 10))
            .join(', ');
    }
}

function updateProcedureSelect() {
    const select = document.getElementById('procedureSelect');
    if (select) {
        select.disabled = selectedTeeth.length === 0;
        if (selectedTeeth.length === 0) {
            select.value = '';
        }
    }
}

window.dentalChart = window.dentalChart || {};
window.dentalChart.getSelectedTeeth = function () {
    return [...selectedTeeth];
};
window.dentalChart.clearSelection = function () {
    selectedTeeth = [];
    updateToothDisplay();
    updateToothInput();
    updateProcedureSelect();
};
window.dentalChart.setSelectedTeeth = function (teeth) {
    selectedTeeth = (teeth || []).map(String);
    updateToothDisplay();
    updateToothInput();
    updateProcedureSelect();
};
window.dentalChart.selectTooth = function (toothNumber) {
    const toothStr = String(toothNumber);
    if (!selectedTeeth.includes(toothStr)) {
        selectedTeeth.push(toothStr);
        updateToothDisplay();
        updateToothInput();
        updateProcedureSelect();
    }
};

if (document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', initDentalChart);
} else {
    initDentalChart();
}

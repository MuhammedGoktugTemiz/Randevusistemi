const patientUpperTeeth = [18, 17, 16, 15, 14, 13, 12, 11, 21, 22, 23, 24, 25, 26, 27, 28];
const patientLowerTeeth = [48, 47, 46, 45, 44, 43, 42, 41, 31, 32, 33, 34, 35, 36, 37, 38];

function createPatientToothElement(number, highlighted, clickable) {
    const wrapper = document.createElement('div');
    wrapper.className = 'patient-tooth' + (highlighted ? ' highlighted' : '');
    if (clickable) wrapper.classList.add('clickable');
    wrapper.dataset.tooth = number;

    const numberSpan = document.createElement('span');
    numberSpan.className = 'patient-tooth-number';
    numberSpan.textContent = number;
    wrapper.appendChild(numberSpan);

    if (highlighted) {
        const check = document.createElement('span');
        check.className = 'patient-tooth-check';
        check.textContent = '✓';
        wrapper.appendChild(check);
    }

    return wrapper;
}

function renderPatientToothRow(teeth, highlightedSet, toothMap, detailHandler) {
    const row = document.createElement('div');
    row.className = 'patient-dental-row';
    teeth.forEach(tooth => {
        const key = String(tooth);
        const hasData = highlightedSet.has(key);
        const element = createPatientToothElement(tooth, hasData, hasData && typeof detailHandler === 'function');

        if (hasData && typeof detailHandler === 'function') {
            element.addEventListener('click', () => detailHandler(key, toothMap.get(key) || []));
        }

        row.appendChild(element);
    });
    return row;
}

window.renderPatientDentalChart = function (containerId, procedureData, detailContainerId) {
    const container = document.getElementById(containerId);
    if (!container) return;
    const detailContainer = detailContainerId ? document.getElementById(detailContainerId) : null;
    const toothMap = new Map();

    (procedureData || []).forEach(group => {
        const procedure = group?.procedure || 'Belirtilmedi';
        (group?.teeth || []).forEach(tooth => {
            const key = String(tooth);
            if (!toothMap.has(key)) toothMap.set(key, []);
            toothMap.get(key).push(procedure);
        });
    });

    const highlighted = new Set(toothMap.keys());

    container.innerHTML = '';
    const rowsWrapper = document.createElement('div');
    rowsWrapper.className = 'patient-dental-rows';
    const detailHandler = detailContainer
        ? (tooth, procedures) => {
              detailContainer.innerHTML = `
                <div class="tooth-detail-head">
                    <div>
                        <strong>Diş ${tooth}</strong>
                        <span>${procedures.length} işlem</span>
                    </div>
                </div>
                <ul class="tooth-detail-list">
                    ${procedures
                        .map(proc => `<li><span class="procedure-dot"></span>${proc}</li>`)
                        .join('')}
                </ul>
            `;
          }
        : null;

    rowsWrapper.appendChild(renderPatientToothRow(patientUpperTeeth, highlighted, toothMap, detailHandler));
    rowsWrapper.appendChild(renderPatientToothRow(patientLowerTeeth, highlighted, toothMap, detailHandler));
    container.appendChild(rowsWrapper);

    if (detailContainer) {
        if (highlighted.size === 0) {
            detailContainer.innerHTML = '<p class="subtext">Bu hastaya ait işaretlenmiş diş bulunmuyor.</p>';
        } else {
            detailContainer.innerHTML = '<p class="subtext">Detay görmek için işaretli dişlerden birini seçin.</p>';
        }
    }
};

window.renderPatientProcedureList = function (containerId, procedureData) {
    const container = document.getElementById(containerId);
    if (!container) return;

    if (!procedureData || procedureData.length === 0) {
        container.innerHTML = '<p class="subtext">Bu hastaya ait işlem kaydı bulunamadı.</p>';
        return;
    }

    container.innerHTML = procedureData.map(group => `
        <div class="patient-procedure-item">
            <div>
                <strong>${group.procedure || 'Belirtilmedi'}</strong>
                <p>${(group.teeth || []).join(', ')}</p>
            </div>
        </div>
    `).join('');
};

